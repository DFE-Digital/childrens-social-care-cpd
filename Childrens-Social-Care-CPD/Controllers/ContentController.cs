using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc;

namespace Childrens_Social_Care_CPD.Controllers;

public class ContentController : Controller
{
    private readonly ICpdContentfulClient _cpdClient;

    public ContentController(ICpdContentfulClient cpdClient)
    {
        _cpdClient = cpdClient;
    }

    private async Task<Content> FetchPageContentAsync(string contentId, CancellationToken cancellationToken)
    {
        var queryBuilder = QueryBuilder<Content>.New
            .ContentTypeIs("content")
            .FieldEquals("fields.id", contentId)
            .Include(10);

        var result = await _cpdClient.GetEntries(queryBuilder, cancellationToken);

        return result?.FirstOrDefault();
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
    [Route("/{*pagename:regex(^[[0-9a-z]](\\/?[[0-9a-z\\-]])*\\/?$)}")] 
    public async Task<IActionResult> Index(CancellationToken cancellationToken, string pageName = "home", bool preferenceSet = false)
    {
        pageName = pageName?.TrimEnd('/');
        var pageContent = await FetchPageContentAsync(pageName, cancellationToken);
        if (pageContent == null)
        {
            return NotFound();
        }

        var contextModel = new ContextModel(
            Id: pageContent.Id,
            Title: pageContent.Title,
            PageName: pageName,
            Category: pageContent.Category,
            UseContainers: pageContent.SideMenu == null,
            PreferenceSet: preferenceSet,
            BackLink: pageContent.BackLink);

        ViewData["ContextModel"] = contextModel;
        ViewData["StateModel"] = new StateModel();
        return View(pageContent);
    }
}