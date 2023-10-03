using Childrens_Social_Care_CPD.Models;
using Microsoft.AspNetCore.Mvc;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Search;
using Childrens_Social_Care_CPD.Contentful;
using System.Text.RegularExpressions;

namespace Childrens_Social_Care_CPD.Controllers;

public class CookieController : Controller
{
    private readonly ICpdContentfulClient _cpdClient;
    private readonly ICookieHelper _cookieHelper;
    private const string _pageName = "cookies";

    public CookieController(ICpdContentfulClient cpdClient, ICookieHelper cookieHelper)
    {
        _cpdClient = cpdClient;
        _cookieHelper = cookieHelper;
    }

    [HttpPost]
    [Route("/cookies/setpreferences")]
    public IActionResult SetPreferences(string consentValue, string sourcePage = null, bool fromCookies = false)
    {
        var consentState = AnalyticsConsentStateHelper.Parse(consentValue);
        _cookieHelper.SetResponseAnalyticsCookieState(HttpContext, consentState);

        if (fromCookies)
        {
            // this just indicates that the post happened from the cookies page
            return RedirectToAction("Cookies", "Cookie", new { sourcePage, preferenceSet = true });
        }
        
        return LocalRedirect($"~/{sourcePage}?preferenceSet=True");
    }

    [HttpGet]
    [Route("/cookies")]
    public async Task<IActionResult> Cookies(CancellationToken cancellationToken, string sourcePage = null, bool preferenceSet = false)
    {
        sourcePage ??= string.Empty;

        var queryBuilder = QueryBuilder<Content>.New
            .ContentTypeIs("content")
            .FieldEquals("fields.id", _pageName)
            .Include(10);

        var result = await _cpdClient.GetEntries(queryBuilder, cancellationToken);
        var pageContent = result.FirstOrDefault();
        
        if (pageContent == null)
        {
            return NotFound();
        }

        if (!Regex.IsMatch(sourcePage, "^[0-9a-z](\\/?[0-9a-z\\-])*\\/?$", RegexOptions.None, TimeSpan.FromMilliseconds(100)))
        {
            sourcePage = "home";
        }

        var consentState = _cookieHelper.GetRequestAnalyticsCookieState(HttpContext);
        var contextModel = new ContextModel(pageContent.Id, pageContent.Title, _pageName, pageContent.Category, true, preferenceSet, true);
        ViewData["ContextModel"] = contextModel;

        var model = new CookiesAndAnalyticsConsentModel
        {
            Content = pageContent,
            ConsentState = consentState,
            SourcePage = sourcePage,
        };

        return View(model);
    }
}