using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core.Models;
using Contentful.Core.Search;
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
    private readonly ILogger<ResourcesController> _logger;
    private readonly ICpdContentfulClient _cpdClient;
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

    public ResourcesController(ILogger<ResourcesController> logger, ICpdContentfulClient cpdClient)
    {
        _logger = logger;
        _cpdClient = cpdClient;
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

    private Task<ContentfulCollection<Content>> FetchResourcesContentAsync()
    {
        var queryBuilder = QueryBuilder<Content>.New
            .ContentTypeIs("content")
            .Include(10)
            .FieldEquals("fields.id", "resources");

        return _cpdClient.GetEntries(queryBuilder);
    }

    private Task<ContentfulCollection<Resource>> FetchResourceSearchResultsAsync(int[] tags, int skip = 0, int limit = 5)
    {
        var queryBuilder = QueryBuilder<Resource>.New
            .ContentTypeIs("resource")
            .Include(1)
            .FieldIncludes("metadata.tags.sys.id", GetQueryTags(tags))
            .OrderBy("-sys.createdAt")
            .Skip(skip)
            .Limit(limit);

        return _cpdClient.GetEntries(queryBuilder);
    }

    private async Task<Tuple<Content, ContentfulCollection<Resource>>> GetContentAsync(int[] tags, int skip = 0, int limit = 5)
    {
        var pageContentTask = FetchResourcesContentAsync();
        var searchContentTask = FetchResourceSearchResultsAsync(tags, skip, limit);

        await Task.WhenAll(pageContentTask, searchContentTask);
        return Tuple.Create(pageContentTask.Result?.FirstOrDefault(), searchContentTask.Result);
    }

    private static Tuple<int, int, int> CalculatePaging(ResourcesQuery query)
    {
        var pageSize = 8;
        var page = Math.Max(query.Page, 1);
        var skip = (page - 1) * pageSize;

        return Tuple.Create(page, skip, pageSize);
    }

    private static string GetPagingFormatString(int[] tags)
    {
        var tagStrings = tags.Select(x => $"tags={x}");
        var qsTags = string.Join("&", tagStrings);

        return string.IsNullOrEmpty(qsTags)
            ? $"/resources?page={{0}}"
            : $"/resources?page={{0}}&{qsTags}";
    }

    [Route("resources", Name = "Resource")]
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] ResourcesQuery query, bool preferencesSet = false)
    {
        query ??= new ResourcesQuery();
        query.Tags ??= Array.Empty<int>();

        (var page, var skip, var pageSize) = CalculatePaging(query);
        (var pageContent, var contentCollection) = await GetContentAsync(query.Tags, skip, pageSize);

        var totalPages = (int)Math.Ceiling((decimal)contentCollection.Total / pageSize);
        page = Math.Min(page, totalPages);

        var contextModel = new ContextModel(string.Empty, "Resources", "Resources", "Resources", true, preferencesSet);
        ViewData["ContextModel"] = contextModel;

        var viewModel = new ResourcesListViewModel(pageContent, contentCollection, _tagInfos, query.Tags, page, totalPages, contentCollection.Total, GetPagingFormatString(query.Tags));
        return View(viewModel);
    }
}