using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

    private static readonly List<TagInfo> _tagInfos = new () { 
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

        try
        {
            return tags.Select(x => _tagInfos[x].TagName);
        }
        catch
        {
            _logger.LogWarning("Passed tag values do not match known values: {Passed Values}", tags);
            throw;
        }
    }

    private async Task<Content> FetchResourcesContentAsync()
    {
        // TODO: we need to have the resources content model
        var queryBuilder = QueryBuilder<Content>.New
            .ContentTypeIs("content")
            .Include(10)
            .FieldEquals("fields.id", "resources");

        var result = await _cpdClient.GetEntries(queryBuilder);
        return result?.FirstOrDefault();
    }

    private Task<ContentfulCollection<Content>> FetchResourceSearchResultsAsync(int[] tags, int skip = 0, int limit = 5)
    {
        // TODO: we need to have the resources content model
        var queryBuilder = QueryBuilder<Content>.New
            .ContentTypeIs("content")
            .Include(1)
            .FieldIncludes("metadata.tags.sys.id", GetQueryTags(tags))
            .OrderBy("-sys.createdAt")
            .Skip(skip)
            .Limit(limit);
        
        return _cpdClient.GetEntries(queryBuilder);
    }

    [Route("resources", Name = "Resource")]
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] ResourcesQuery query, bool preferencesSet = false)
    {
        // TODO: we should probably query for a resources object to enable content management of the banner
        query ??= new ResourcesQuery();
        query.Tags ??= Array.Empty<int>();

        var pageSize = 8;
        var page = query.Page;
        page = Math.Max(page, 1);
        var skip = (page - 1) * pageSize;
        var pageContent = await FetchResourcesContentAsync();

        var contentCollection = await FetchResourceSearchResultsAsync(query.Tags, skip, pageSize);
        var totalPages = (int)Math.Ceiling((decimal)contentCollection.Total / pageSize);
        page = Math.Min(page, totalPages);

        var contextModel = new ContextModel(string.Empty, "Resources", "Resources", "Resources", true, preferencesSet);
        ViewData["ContextModel"] = contextModel;

        var viewModel = new ResourcesListViewModel(pageContent, contentCollection, _tagInfos, query.Tags, page, totalPages, contentCollection.Total);
        return View(viewModel);
    }
}