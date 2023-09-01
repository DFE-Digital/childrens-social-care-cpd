using Childrens_Social_Care_CPD;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace Childrens_Social_Care_CPD_Tests.Controllers;

public class AppInfoControllerTests
{
    private AppInfoController _controller;
    private IApplicationConfiguration _applicationConfiguration;

    [SetUp]
    public void Setup()
    {
        _applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        _controller = new AppInfoController(_applicationConfiguration);
    }

    [Test]
    public void AppInfo_Includes_Contentful_Environment()
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
    public void AppInfo_Includes_Azure_Environment()
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
    public void AppInfo_Includes_Git_Hash()
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
    public void AppInfo_Includes_App_Version()
    {
        // arrange
        var value = "foo";
        _applicationConfiguration.AppVersion.Returns(value);

        // act
        var actual = _controller.AppInfo().Value as ApplicationInfo;

        // assert
        actual.Version.Should().Be(value);
    }
}