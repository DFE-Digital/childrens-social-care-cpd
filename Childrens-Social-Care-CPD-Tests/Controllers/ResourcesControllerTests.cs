using Castle.Core.Logging;
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
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.Controllers;

public class ResourcesControllerTests
{
    private ResourcesController _resourcesController;
    private IRequestCookieCollection _cookies;
    private HttpContext _httpContext;
    private HttpRequest _httpRequest;
    private ICpdContentfulClient _contentfulClient;
    private ILogger<ResourcesController> _logger;
    private CancellationTokenSource _cancellationTokenSource;

    private void SetContent(Content content, ContentfulCollection<Resource> resourceCollection)
    {
        resourceCollection ??= new ();

        _contentfulClient
            .GetEntries(Arg.Any<QueryBuilder<Resource>>(), Arg.Any<CancellationToken>())
            .Returns(resourceCollection);

        var contentCollection = new ContentfulCollection<Content>();

        contentCollection.Items = content == null
            ? new List<Content>()
            : contentCollection.Items = new List<Content> { content };

        _contentfulClient
            .GetEntries(Arg.Any<QueryBuilder<Content>>(), Arg.Any<CancellationToken>())
            .Returns(contentCollection);

        _cancellationTokenSource = new CancellationTokenSource();
    }

    [SetUp]
    public void SetUp()
    {
        _logger = Substitute.For<ILogger<ResourcesController>>();
        _cookies = Substitute.For<IRequestCookieCollection>();
        _httpContext = Substitute.For<HttpContext>();
        _httpRequest = Substitute.For<HttpRequest>();
        var controllerContext = Substitute.For<ControllerContext>();

        _httpRequest.Cookies.Returns(_cookies);
        _httpContext.Request.Returns(_httpRequest);
        controllerContext.HttpContext = _httpContext;

        _contentfulClient = Substitute.For<ICpdContentfulClient>();

        _resourcesController = new ResourcesController(_logger, _contentfulClient)
        {
            ControllerContext = controllerContext,
            TempData = Substitute.For<ITempDataDictionary>()
        };
    }

    [Test]
    public async Task Search_With_Empty_Query_Returns_View()
    {
        // arrange
        SetContent(null, null);

        // act
        var actual = await _resourcesController.Search(query: null, _cancellationTokenSource.Token) as ViewResult;

        // assert
        actual.Should().BeOfType<ViewResult>();
        actual.Model.Should().BeOfType<ResourcesListViewModel>();
    }

    [Test]
    public async Task Search_Page_Resource_Is_Passed_To_View()
    {
        // arrange
        var content = new Content();
        SetContent(content, null);

        // act
        var actual = (await _resourcesController.Search(query: null, _cancellationTokenSource.Token) as ViewResult)?.Model as ResourcesListViewModel;

        // assert
        actual.Content.Should().Be(content);
    }

    [Test]
    public async Task Search_Sets_The_ViewState_ContextModel()
    {
        // arrange
        SetContent(null, null);

        // act
        await _resourcesController.Search(null, _cancellationTokenSource.Token);
        var actual = _resourcesController.ViewData["ContextModel"] as ContextModel;

        // assert
        actual.Should().NotBeNull();
        actual.Id.Should().Be(string.Empty);
        actual.Title.Should().Be("Resources");
        actual.Category.Should().Be("Resources");
    }

    [Test]
    public async Task Search_Selected_Tags_Are_Passed_Into_View()
    {
        // arrange
        SetContent(null, null);
        var query = new ResourcesQuery
        {
            Page = 1,
            Tags = new int[] { 1, 2 }
        };

        // act
        var actual = (await _resourcesController.Search(query, _cancellationTokenSource.Token) as ViewResult)?.Model as ResourcesListViewModel;

        // assert
        actual.SelectedTags.Should().Equal(query.Tags);
    }

    [Test]
    public async Task Search_Page_Set_To_Be_In_Bounds()
    {
        // arrange
        SetContent(null, new ContentfulCollection<Resource> {
            Items = new List<Resource> {
                new Resource(),
                new Resource(),
                new Resource(),
            },
            Total = 3
        });
        var query = new ResourcesQuery
        {
            Page = 2,
            Tags = new int[] { 1, 2 }
        };

        // act
        var actual = (await _resourcesController.Search(query, _cancellationTokenSource.Token) as ViewResult)?.Model as ResourcesListViewModel;

        // assert
        actual.CurrentPage.Should().Be(1);
    }

    [Test]
    public async Task Search_Invalid_Tags_Logs_Warning()
    {
        // arrange
        SetContent(null, null);
        var tags = new int[] { -1 };
        var query = new ResourcesQuery
        {
            Page = 2,
            Tags = tags
        };

        // act
        await _resourcesController.Search(query, _cancellationTokenSource.Token);
        
        //assert
        _logger.ReceivedWithAnyArgs(1).LogWarning(default, args: default);
    }
}