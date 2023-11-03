using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.DataAccess;
using Childrens_Social_Care_CPD.Extensions;
using Childrens_Social_Care_CPD.GraphQL.Queries;
using Childrens_Social_Care_CPD.Models;

namespace Childrens_Social_Care_CPD.Core.Resources;

internal class ResourcesFixedTagsSearchStrategy : IResourcesSearchStrategy
{
    private const int PAGE_SIZE = 8;
    private readonly ILogger _logger;
    private readonly IResourcesRepository _resourcesRepository;
    private static readonly List<TagInfo> _tagInfos = new() {
        new TagInfo("Type", "Case studies", "caseStudies"),
        new TagInfo("Type", "CPD", "cpd"),
        new TagInfo("Type", "Direct tools", "directTools"),
        new TagInfo("Type", "Knowledge articles", "knowledgeArticles"),
        new TagInfo("Career stage", "Practitioner", "practitioner"),
        new TagInfo("Career stage", "Experienced practitioner", "experiencedPractitioner"),
        new TagInfo("Career stage", "Manager", "manager"),
    };
    private static readonly HashSet<string> _allTags = _tagInfos.Select(x => x.TagName).ToHashSet();

    public ResourcesFixedTagsSearchStrategy(IResourcesRepository resourcesRepository, ILogger<ResourcesFixedTagsSearchStrategy> logger)
    {
        _resourcesRepository = resourcesRepository;
        _logger = logger;
    }

    private IEnumerable<string> GetQueryTags(int[] tags)
    {
        if (tags.Length == 0)
        {
            return _allTags;
        }

        if (Array.Exists(tags, x => x < 0 || x >= _tagInfos.Count))
        {
            _logger.LogWarning("Passed tag values do not match known values: {Passed Values}", tags);
            return Array.Empty<string>();
        }

        return tags.Select(x => { return _tagInfos[x].TagName; });
    }

    private static Tuple<int, int, int> CalculatePageStats(SearchResourcesByTags.ResponseType searchResults, int page)
    {
        var totalResults = searchResults?.ResourceCollection?.Total ?? 0;
        var totalPages = (int)Math.Ceiling((decimal)totalResults / PAGE_SIZE);

        return Tuple.Create(totalResults, totalPages, Math.Min(page, totalPages));
    }

    private static string GetPagingFormatString(int[] tags)
    {
        if (tags.Any())
        {
            var tagStrings = tags.Select(x => $"tags={x}");
            var allTags = string.Join("&", tagStrings);
            return $"/resources-learning?page={{0}}&{allTags}";
        }

        return $"/resources-learning?page={{0}}";
    }

    public async Task<ResourcesListViewModel> SearchAsync(ResourcesQuery query, CancellationToken cancellationToken = default)
    {
        query ??= new ResourcesQuery();
        var queryTags = query.Tags.Select(x => int.TryParse(x, out var value) ? value : 0).ToHashSet().ToArray();

        var page = Math.Max(query.Page, 1);
        var skip = (page - 1) * PAGE_SIZE;
        var pageContentTask = _resourcesRepository.FetchRootPageAsync(cancellationToken);
        var searchResults = await _resourcesRepository.FindByTagsAsync(GetQueryTags(queryTags), skip, PAGE_SIZE, query.SortOrder, cancellationToken);
        var pageContent = await pageContentTask;
        (var totalResults, var totalPages, var currentPage) = CalculatePageStats(searchResults, page);

        int startRecord = 0;
        if (totalResults > 0)
        {
            startRecord = ((currentPage * PAGE_SIZE) - PAGE_SIZE) + 1;
        }

        return new ResourcesListViewModel(
            pageContent,
            searchResults?.ResourceCollection,
            _tagInfos,
            queryTags.Select(x => x.ToString()),
            (int)query.SortOrder,
            startRecord,
            currentPage,
            totalPages,
            totalResults,
            GetPagingFormatString(queryTags)
        );
    }
}