using Childrens_Social_Care_CPD;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Models;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.Controllers;

public class ContentControllerBreadcrumbTests
{
    private ContentController _contentController;
    private IRequestCookieCollection _cookies;
    private HttpContext _httpContext;
    private HttpRequest _httpRequest;
    private ICpdContentfulClient _contentfulClient;

    private void SetContent(List<KeyValuePair<string, Content>> content)
    {
        var contentCollections = new List<KeyValuePair<string, ContentfulCollection<Content>>>();
        foreach (var contentDefinition in content)
        {
            var contentCollection = new ContentfulCollection<Content>();

            contentCollection.Items = contentDefinition.Value == null
                ? new List<Content>()
                : contentCollection.Items = new List<Content> { contentDefinition.Value };

            contentCollections.Add(new KeyValuePair<string, ContentfulCollection<Content>>(contentDefinition.Key, contentCollection));
        }

        _contentfulClient
            .GetEntries(Arg.Any<QueryBuilder<Content>>(), Arg.Any<CancellationToken>())
            .Returns(x => {
                var query = x.Arg<QueryBuilder<Content>>().Build();
                foreach (var contentDefinition in content)
                {
                    if (query.Contains("fields.id=" + contentDefinition.Key)) return contentCollections.First(x => x.Key == contentDefinition.Key).Value;
                }
                return new ContentfulCollection<Content>();
            });
        
    }

    private void SetContent() {
        var page = new Content()
        {
            Id = "page",
            Title = "Content Page"
        };
        
        var content = new List<KeyValuePair<string, Content>>()
        {
            new KeyValuePair<string, Content>(page.Id, page),
        };

        SetContent(content);        
    }

    [SetUp]
    public void SetUp()
    {
        _cookies = Substitute.For<IRequestCookieCollection>();
        _httpContext = Substitute.For<HttpContext>();
        _httpRequest = Substitute.For<HttpRequest>();
        var controllerContext = Substitute.For<ControllerContext>();

        _httpRequest.Cookies.Returns(_cookies);
        _httpContext.Request.Returns(_httpRequest);
        
        controllerContext.HttpContext = _httpContext;

        _contentfulClient = Substitute.For<ICpdContentfulClient>();

        _contentController = new ContentController(_contentfulClient)
        {
            ControllerContext = controllerContext,
            TempData = Substitute.For<ITempDataDictionary>()
        };
    }

    [TearDown]
    public void TearDown()
    {
        _contentfulClient = Substitute.For<ICpdContentfulClient>();
    }

    [Test]
    public async Task Index_Sets_Breadcrumbs_In_ContextModel()
    {
        // arrange
        var parentPage = new Content()
        {
            Id = "parent",
            Title = "Parent Page"
        };
        var childPage = new Content(){
            Id = "child",
            Title = "Child Page",
            ParentPages = new List<Content>(){parentPage}
        };

        var content = new List<KeyValuePair<string, Content>>(){
            new KeyValuePair<string, Content>(parentPage.Id, parentPage),
            new KeyValuePair<string, Content>(childPage.Id, childPage)
        };

        SetContent(content);

        // act
        await _contentController.Index("child");
        var actual = _contentController.ViewData["ContextModel"] as ContextModel;
        var breadcrumbTrail = actual?.BreadcrumbTrail;

        // assert
        actual.Should().NotBeNull();
        breadcrumbTrail[0].Key.Should().Be("Child Page");
        breadcrumbTrail[0].Value.Should().Be("child");
        breadcrumbTrail[1].Key.Should().Be("Parent Page");
        breadcrumbTrail[1].Value.Should().Be("parent");
    }

    [Test]
    public async Task Index_Sets_Blank_Breadcrumbs_In_ContextModel_If_Page_Has_No_Parent()
    {
        // arrange
        SetContent();

        // act
        await _contentController.Index("page");
        var actual = _contentController.ViewData["ContextModel"] as ContextModel;
        var breadcrumbTrail = actual?.BreadcrumbTrail;

        // assert
        actual.Should().NotBeNull();
        breadcrumbTrail.Should().BeEmpty();
    }

    [Test]
    public async Task Index_Sets_Breadcrumbs_Where_Page_Has_Multiple_Parents()
    {
        // arrange
        var parentPage1 = new Content()
        {
            Id = "parent1",
            Title = "First Parent Page"
        };
        var parentPage2 = new Content()
        {
            Id = "parent2",
            Title = "Second Parent Page"
        };
        var childPage = new Content(){
            Id = "child",
            Title = "Child Page",
            ParentPages = new List<Content>(){parentPage1, parentPage2}
        };

        var content = new List<KeyValuePair<string, Content>>(){
            new KeyValuePair<string, Content>(parentPage1.Id, parentPage1),
            new KeyValuePair<string, Content>(parentPage2.Id, parentPage2),
            new KeyValuePair<string, Content>(childPage.Id, childPage)
        };

        SetContent(content);

        // act
        await _contentController.Index("child");
        var actual = _contentController.ViewData["ContextModel"] as ContextModel;
        var breadcrumbTrail = actual?.BreadcrumbTrail;

        // assert
        actual.Should().NotBeNull();
        breadcrumbTrail[0].Key.Should().Be("Child Page");
        breadcrumbTrail[0].Value.Should().Be("child");
        breadcrumbTrail[1].Key.Should().Be("First Parent Page");
        breadcrumbTrail[1].Value.Should().Be("parent1");
    }
}
