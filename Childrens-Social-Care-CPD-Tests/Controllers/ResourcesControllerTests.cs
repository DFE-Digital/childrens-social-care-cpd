using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Core.Resources;
using Childrens_Social_Care_CPD.DataAccess;
using Childrens_Social_Care_CPD.GraphQL.Queries;
using Childrens_Social_Care_CPD.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.Controllers;

public class ResourcesControllerTests
{
    private IFeaturesConfig _featuresConfig;
    private IResourcesRepository _resourcesRepository;
    private ResourcesController _resourcesController;
    private IRequestCookieCollection _cookies;
    private HttpContext _httpContext;
    private HttpRequest _httpRequest;

    [SetUp]
    public void SetUp()
    {
        _resourcesRepository = Substitute.For<IResourcesRepository>();
        _cookies = Substitute.For<IRequestCookieCollection>();
        _httpContext = Substitute.For<HttpContext>();
        _httpRequest = Substitute.For<HttpRequest>();
        var controllerContext = Substitute.For<ControllerContext>();

        _httpRequest.Cookies.Returns(_cookies);
        _httpContext.Request.Returns(_httpRequest);
        controllerContext.HttpContext = _httpContext;

        _featuresConfig = Substitute.For<IFeaturesConfig>();
        _featuresConfig.IsEnabled(Features.ResourcesAndLearning).Returns(true);
        _resourcesController = new ResourcesController(_featuresConfig, _resourcesRepository)
        {
            ControllerContext = controllerContext,
            TempData = Substitute.For<ITempDataDictionary>()
        };
    }

    [Test]
    public async Task Index_Returns_404_When_Resource_Feature_Disabled()
    {
        // arrange
        _featuresConfig.IsEnabled(Features.ResourcesAndLearning).Returns(false);

        // act
        var actual = await _resourcesController.Index();

        // assert
        actual.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Index_Searches_For_Page_Under_Resources_Area()
    {
        // arrange
        var actual = string.Empty;
        _resourcesRepository.GetByIdAsync(Arg.Do<string>(x => actual = x), cancellationToken: Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Tuple.Create((Content)null, (GetContentTags.ResponseType)null)));
        
        // act
        await _resourcesController.Index("foo");

        // assert
        actual.Should().Be("resources-learning/foo");
    }

    [Test]
    public async Task Index_Returns_Not_Found_When_Content_Does_Not_Exist()
    {
        // arrange
        _resourcesRepository.GetByIdAsync(Arg.Any<string>(), cancellationToken: Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Tuple.Create((Content)null, (GetContentTags.ResponseType)null)));

        // act
        var actual = await _resourcesController.Index("foo");

        // assert
        actual.Should().BeOfType<NotFoundResult>();
    }

    private static GetContentTags.ResponseType CreateTagsResponse(List<GetContentTags.Tag> tags)
    {
        return new GetContentTags.ResponseType
        {
            ContentCollection = new()
            {
                Items = new[]
                {
                    new GetContentTags.ContentItem
                    {
                        ContentfulMetaData = new()
                        {
                            Tags = tags
                        }
                    }
                }
            }
        };
    }

    [TestCase("Published")]
    [TestCase("Last updated")]
    public async Task Index_Passes_Default_Properties(string propertyName)
    {
        // arrange
        var createdAt = DateTime.UtcNow.AddMinutes(-10);
        var updatedAt = DateTime.UtcNow;
        var content = new Content
        {
            Sys = new()
            {
                CreatedAt = createdAt,
                UpdatedAt = updatedAt,
            }
        };
        var tags = CreateTagsResponse(new());

        _resourcesRepository.GetByIdAsync(Arg.Any<string>(), cancellationToken: Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Tuple.Create(content, tags)));

        // act
        var result = await _resourcesController.Index("foo") as ViewResult;
        var properties = result.ViewData["Properties"] as IDictionary<string, string>;

        // assert
        properties.Should().NotBeNull();
        properties.ContainsKey(propertyName).Should().BeTrue();
    }

    [Test]
    public async Task Index_Passes_Correctly_Tagged_Properties()
    {
        // arrange
        var createdAt = DateTime.UtcNow.AddMinutes(-10);
        var updatedAt = DateTime.UtcNow;
        var content = new Content
        {
            Sys = new ()
            {
                CreatedAt = createdAt,
                UpdatedAt = updatedAt,
            }
        };
        var tags = CreateTagsResponse(new() { new () { Id = "foo", Name = "Resource:Foo=foo" }, });

        _resourcesRepository.GetByIdAsync(Arg.Any<string>(), cancellationToken: Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Tuple.Create(content, tags)));

        // act
        var result = await _resourcesController.Index("foo") as ViewResult;
        var properties = result.ViewData["Properties"] as IDictionary<string, string>;

        // assert
        properties.Should().NotBeNull();
        properties["Foo"].Should().Be("foo");
    }

    [Test]
    public async Task Index_Ignores_Unrelated_Tags_For_Properties()
    {
        // arrange
        var createdAt = DateTime.UtcNow.AddMinutes(-10);
        var updatedAt = DateTime.UtcNow;
        var content = new Content
        {
            Sys = new()
            {
                CreatedAt = createdAt,
                UpdatedAt = updatedAt,
            }
        };
        var tags = CreateTagsResponse(new() { new() { Id = "foo", Name = "Topic:Foo=foo" }, });

        _resourcesRepository.GetByIdAsync(Arg.Any<string>(), cancellationToken: Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Tuple.Create(content, tags)));

        // act
        var result = await _resourcesController.Index("foo") as ViewResult;
        var properties = result.ViewData["Properties"] as IDictionary<string, string>;

        // assert
        properties.Should().NotBeNull();
        properties.ContainsKey("Foo").Should().BeFalse();
    }
}