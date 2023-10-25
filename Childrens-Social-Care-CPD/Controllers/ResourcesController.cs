using Childrens_Social_Care_CPD.DataAccess;
using Childrens_Social_Care_CPD.GraphQL.Queries;
using Childrens_Social_Care_CPD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Childrens_Social_Care_CPD.Controllers;

public record TagInfo(string Category, string DisplayName, string TagName);

public class ResourcesQuery
{
    public int[] Tags { get; set; }
    public int Page { get; set; } = 1;
}

public partial class ResourcesController : Controller
{
    private const int PAGE_SIZE = 8;
    private readonly ILogger<ResourcesController> _logger;
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
    private static readonly IEnumerable<string> _allTags = _tagInfos.Select(x => x.TagName);

    public ResourcesController(ILogger<ResourcesController> logger, IResourcesRepository resourcesRepository)
    {
        _logger = logger;
        _resourcesRepository = resourcesRepository;
    }

    private IEnumerable<string> GetQueryTags(int[] tags)
    {
        if (tags.Length == 0)
        {
            return _allTags;
        }
        
        if (tags.Any(x => (x < 0) || (x >= _tagInfos.Count)))
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
            return $"/resources?page={{0}}&{allTags}";
        }

        return $"/resources?page={{0}}";
    }

    [Route("resources", Name = "Resource")]
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] ResourcesQuery query, CancellationToken cancellationToken, bool preferencesSet = false)
    {
        query ??= new ResourcesQuery();
        query.Tags ??= Array.Empty<int>();

        var page = Math.Max(query.Page, 1);
        var skip = (page - 1) * PAGE_SIZE;
        var pageContentTask = _resourcesRepository.FetchRootPage(cancellationToken);
        var searchResults = await _resourcesRepository.FindByTags(GetQueryTags(query.Tags), skip, PAGE_SIZE, cancellationToken);
        var pageContent = await pageContentTask;
        (var totalResults, var totalPages, var currentPage) = CalculatePageStats(searchResults, page);

        var contextModel = new ContextModel(string.Empty, "Resources", "Resources", "Resources", true, preferencesSet);
        ViewData["ContextModel"] = contextModel;

        var viewModel = new ResourcesListViewModel(
            pageContent,
            searchResults?.ResourceCollection,
            _tagInfos,
            query.Tags,
            currentPage,
            totalPages,
            totalResults,
            GetPagingFormatString(query.Tags)
        );

        return View(viewModel);
    }
}