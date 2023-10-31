using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Core.Resources;
using Childrens_Social_Care_CPD.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.Controllers;

public class ResourcesControllerTests
{
    private IResourcesSearchStrategy _searchStrategy;
    private ResourcesController _resourcesController;
    private IRequestCookieCollection _cookies;
    private HttpContext _httpContext;
    private HttpRequest _httpRequest;

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

        _searchStrategy = Substitute.For<IResourcesSearchStrategy>();
        _resourcesController = new ResourcesController(_searchStrategy)
        {
            ControllerContext = controllerContext,
            TempData = Substitute.For<ITempDataDictionary>()
        };
    }

    [Test]
    public async Task Search_Returns_Strategy_Model()
    {
        // arrange
        var model = new ResourcesListViewModel(null, null, null, null);
        _searchStrategy.SearchAsync(Arg.Any<ResourcesQuery>(), Arg.Any<CancellationToken>()).Returns(model);

        // act
        var actual = await _resourcesController.Search(query: null) as ViewResult;

        // assert
        actual.Model.Should().Be(model);
    }
}