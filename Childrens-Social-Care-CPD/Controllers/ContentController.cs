using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Childrens_Social_Care_CPD.Controllers;

public class ContentController(ICpdContentfulClient cpdClient) : Controller
{
    private async Task<Content> FetchPageContentAsync(string contentId, CancellationToken cancellationToken)
    {
        var queryBuilder = QueryBuilder<Content>.New
            .ContentTypeIs("content")
            .FieldEquals("fields.id", contentId)
            .Include(10);

        var result = await cpdClient.GetEntries(queryBuilder, cancellationToken);

        return result?.FirstOrDefault();
    }

    private async Task<Stack<KeyValuePair<string, string>>> BuildBreadcrumbTrail(List<string> pagesVisited)
    {
        return new Stack<KeyValuePair<string, string>>();
    }

    [HttpGet]
    [Route("/")]
    /*
        Filter permissable page name format. Basically only accept:
            foo
            foo/bar
            foo/bar/xyz
        Reject
            /foo
            foo'bar
            Foo
            <=
        Etc.
    */
    [Route("/{*pagename:regex(^[[0-9a-z]]+[[0-9a-z\\/\\-]]*$)}")]
    public async Task<IActionResult> Index(string pageName = "home", bool preferenceSet = false, bool fs = false, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        pageName = pageName?.TrimEnd('/');
        var content = await FetchPageContentAsync(pageName, cancellationToken);
        if (content == null)
        {
            return NotFound();
        }

        var pagesVisited = HttpContext.Session.Get<List<string>>("pagesVisited");
        if (pagesVisited == null) pagesVisited = new List<string>();
        pagesVisited.Add(pageName);
        HttpContext.Session.Set<List<string>>("pagesVisited", pagesVisited);
        Console.WriteLine("\n\nPages Visited: " + JsonConvert.SerializeObject(pagesVisited));

        var contextModel = new ContextModel(
            Id: content.Id,
            Title: content.Title,
            PageName: pageName,
            Category: content.Category,
            UseContainers: content.Navigation == null,
            PreferenceSet: preferenceSet,
            BackLink: content.BackLink,
            FeedbackSubmitted: fs,
            BreadcrumbTrail: await BuildBreadcrumbTrail(pagesVisited));

        ViewData["ContextModel"] = contextModel;
        ViewData["StateModel"] = new StateModel();

        return View(content);
    }
}