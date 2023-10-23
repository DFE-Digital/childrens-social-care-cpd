using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Models;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Childrens_Social_Care_CPD_Tests.Configuration;
using Microsoft.AspNetCore.Mvc;
using Childrens_Social_Care_CPD.Contentful;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net;
using System.Net.Mime;
using Microsoft.Extensions.Primitives;

namespace Childrens_Social_Care_CPD_Tests.Controllers;

public class ApplicationControllerTests
{
    private ApplicationController _controller;
    private IApplicationConfiguration _applicationConfiguration;

    [SetUp]
    public void Setup()
    {
        _applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        _controller = new ApplicationController(_applicationConfiguration);
    }

    [Test]
    public void ApplicationController_Includes_Contentful_Environment()
    {
        // arrange
        var value = "foo";
        _applicationConfiguration.ContentfulEnvironment.Returns(value);

        // act
        var actual = _controller.AppInfo().Value as ApplicationInfo;

        // assert
        actual.ContentfulEnvironment.Should().Be(value);
    }

    [Test]
    public void ApplicationController_Includes_Azure_Environment()
    {
        // arrange
        var value = "foo";
        _applicationConfiguration.AzureEnvironment.Returns(value);

        // act
        var actual = _controller.AppInfo().Value as ApplicationInfo;

        // assert
        actual.Environment.Should().Be(value);
    }

    [Test]
    public void ApplicationController_Includes_Git_Hash()
    {
        // arrange
        var value = "foo";
        _applicationConfiguration.GitHash.Returns(value);

        // act
        var actual = _controller.AppInfo().Value as ApplicationInfo;

        // assert
        actual.GitShortHash.Should().Be(value);
    }

    [Test]
    public void ApplicationController_Includes_App_Version()
    {
        // arrange
        var value = "foo";
        _applicationConfiguration.AppVersion.Returns(value);

        // act
        var actual = _controller.AppInfo().Value as ApplicationInfo;

        // assert
        actual.Version.Should().Be(value);
    }


    [Test]
    public void ApplicationController_Configuration_Returns_Json()
    {
        // arrange
        var httpRequest = Substitute.For<HttpRequest>();
        httpRequest.Headers.Accept.Returns(new StringValues(MediaTypeNames.Application.Json));
        
        var httpContext = Substitute.For<HttpContext>();
        httpContext.Request.Returns(httpRequest);
        
        var controllerContext = Substitute.For<ControllerContext>();
        controllerContext.HttpContext = httpContext;

        var mockConfig = new TestConfigurationMock();
        var controller = new ApplicationController(mockConfig)
        {
            ControllerContext = controllerContext,
            TempData = Substitute.For<ITempDataDictionary>()
        };

        // act
        var actual = controller.Configuration() as JsonResult;

        // assert
        actual.Should().NotBeNull();
    }

    [Test]
    public void ApplicationController_Configuration_Returns_Html()
    {
        // arrange
        var httpRequest = Substitute.For<HttpRequest>();
        httpRequest.Headers.Accept.Returns(new StringValues(MediaTypeNames.Text.Html));

        var httpContext = Substitute.For<HttpContext>();
        httpContext.Request.Returns(httpRequest);
        
        var controllerContext = Substitute.For<ControllerContext>();
        controllerContext.HttpContext = httpContext;

        var mockConfig = new TestConfigurationMock();
        var controller = new ApplicationController(mockConfig)
        {
            ControllerContext = controllerContext,
            TempData = Substitute.For<ITempDataDictionary>()
        };

        // act
        var actual = controller.Configuration() as ViewResult;

        // assert
        actual.Should().NotBeNull();
    }
}