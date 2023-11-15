using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.DataAccess;
using Childrens_Social_Care_CPD.GraphQL.Queries;
using Childrens_Social_Care_CPD.Models;

namespace Childrens_Social_Care_CPD.Core.Resources;

internal class ResourcesDynamicTagsSearchStategy : IResourcesSearchStrategy
{
    private const int PAGE_SIZE = 8;
    private readonly IResourcesRepository _resourcesRepository;

    public ResourcesDynamicTagsSearchStategy(IResourcesRepository resourcesRepository)
    {
        _resourcesRepository = resourcesRepository;
    }

    private static IEnumerable<string> GetQueryTags(string[] tags, HashSet<string> tagIds)
    {
        if (tags.Length == 0)
        {
            return tagIds;
        }

        return tagIds.Where(x => tags.Contains(x));
    }

    private static IEnumerable<string> SanitiseTags(IEnumerable<string> tags, HashSet<string> tagIds)
    {
        return tagIds.Where(x => tags.Contains(x));
    }

    private static Tuple<int, int, int> CalculatePageStats(SearchResourcesByTags.ResponseType searchResults, int page)
    {
        var totalResults = searchResults?.ContentCollection?.Total ?? 0;
        var totalPages = (int)Math.Ceiling((decimal)totalResults / PAGE_SIZE);

        return Tuple.Create(totalResults, totalPages, Math.Min(page, totalPages));
    }

    private static string GetPagingFormatString(IEnumerable<string> tags, ResourceSortOrder sortOrder)
    {
        var result = $"/resources-learning?page={{0}}&sortOrder={(int)sortOrder}";

        if (tags.Any())
        {
            var tagStrings = tags.Select(x => $"tags={x}");
            var allTags = string.Join("&", tagStrings);
            result += $"&{allTags}";
        }

        return result;
    }

    public async Task<ResourcesListViewModel> SearchAsync(ResourcesQuery query, CancellationToken cancellationToken = default)
    {
        query ??= new ResourcesQuery();

        var tagInfos = await _resourcesRepository.GetSearchTagsAsync();
        var tagIds = new HashSet<string>(tagInfos.Select(x => x.TagName));
        query.Tags = SanitiseTags(query.Tags, tagIds).ToArray();

        var page = Math.Max(query.Page, 1);
        var skip = (page - 1) * PAGE_SIZE;

        var pageContentTask = _resourcesRepository.FetchRootPageAsync(cancellationToken);
        var searchResults = await _resourcesRepository.FindByTagsAsync(GetQueryTags(query.Tags, tagIds), skip, PAGE_SIZE, query.SortOrder, cancellationToken);
        var pageContent = await pageContentTask;
        (var totalResults, var totalPages, var currentPage) = CalculatePageStats(searchResults, page);

        int startRecord = 0;
        if (totalResults > 0)
        {
            startRecord = ((currentPage * PAGE_SIZE) - PAGE_SIZE) + 1;
        }
       
        return new ResourcesListViewModel(
            pageContent,
            searchResults?.ContentCollection,
            tagInfos,
            query.Tags,
            (int)query.SortOrder,
            startRecord,
            currentPage,
            totalPages,
            totalResults,
            GetPagingFormatString(query.Tags, query.SortOrder)
        );
    }
}