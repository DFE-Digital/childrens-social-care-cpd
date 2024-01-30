using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.DataAccess;
using Childrens_Social_Care_CPD.GraphQL.Queries;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.Controllers;

public class FeedbackControllerTests
{
    private IFeaturesConfig _featuresConfig;
    private FeedbackController _feedbackController;
    private IRequestCookieCollection _cookies;
    private HttpContext _httpContext;
    private HttpRequest _httpRequest;
    private ICpdContentfulClient _contentfulClient;

    [SetUp]
    public void SetUp()
    {
        _contentfulClient = Substitute.For<ICpdContentfulClient>();

        _cookies = Substitute.For<IRequestCookieCollection>();
        _httpContext = Substitute.For<HttpContext>();
        _httpRequest = Substitute.For<HttpRequest>();
        var controllerContext = Substitute.For<ControllerContext>();

        _httpRequest.Cookies.Returns(_cookies);
        _httpContext.Request.Returns(_httpRequest);
        controllerContext.HttpContext = _httpContext;

        _featuresConfig = Substitute.For<IFeaturesConfig>();
        _featuresConfig.IsEnabled(Features.FeedbackControl).Returns(true);
        _feedbackController = new FeedbackController(_featuresConfig, _contentfulClient)
        {
            ControllerContext = controllerContext,
            TempData = Substitute.For<ITempDataDictionary>()
        };
    }

    [Test]
    public async Task Feedback_Returns_404_When_Resource_Feature_Disabled()
    {
        // arrange
        _featuresConfig.IsEnabled(Features.FeedbackControl).Returns(false);

        // act
        var actual = await _feedbackController.Feedback(new FeedbackModel());

        // assert
        actual.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task JsonFeedback_Returns_404_When_Resource_Feature_Disabled()
    {
        // arrange
        _featuresConfig.IsEnabled(Features.FeedbackControl).Returns(false);

        // act
        var actual = await _feedbackController.JsonFeedback(new FeedbackModel());

        // assert
        actual.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Feedback_Redirects_To_PageId_When_Page_Exists()
    {
        // arrange
        var content = new Content()
        {
            Id = "fooId"
        };

        _contentfulClient
            .GetEntries(Arg.Any<QueryBuilder<Content>>(), Arg.Any<CancellationToken>())
            .Returns(new ContentfulCollection<Content>() { Items = new List<Content>() { content } });

        var model = new FeedbackModel()
        {
            Page = "foo"
        };

        // act
        var actual = await _feedbackController.Feedback(model) as RedirectResult;

        // assert
        actual.Should().NotBeNull();
        actual.Url.Should().StartWith("~/fooId?fs=true");
    }

    [Test]
    public async Task Feedback_Returns_BadRequest_When_Page_Does_Not_Exist()
    {
        // arrange
        _contentfulClient
            .GetEntries(Arg.Any<QueryBuilder<Content>>(), Arg.Any<CancellationToken>())
            .Returns(new ContentfulCollection<Content>() { Items = new List<Content>() });

        var model = new FeedbackModel()
        {
            Page = "foo"
        };

        // act
        var actual = await _feedbackController.Feedback(model) as BadRequestResult;

        // assert
        actual.Should().NotBeNull();
    }

    [Test]
    public async Task JsonFeedback_Returns_BadRequest_When_Page_Does_Not_Exist()
    {
        // arrange
        _contentfulClient
            .GetEntries(Arg.Any<QueryBuilder<Content>>(), Arg.Any<CancellationToken>())
            .Returns(new ContentfulCollection<Content>() { Items = new List<Content>() });

        var model = new FeedbackModel()
        {
            Page = "foo"
        };

        // act
        var actual = await _feedbackController.JsonFeedback(model) as BadRequestResult;

        // assert
        actual.Should().NotBeNull();
    }

    [Test]
    public async Task JsonFeedback_Returns_Ok_When_Page_Exists()
    {
        // arrange
        var content = new Content()
        {
            Id = "fooId"
        };

        _contentfulClient
            .GetEntries(Arg.Any<QueryBuilder<Content>>(), Arg.Any<CancellationToken>())
            .Returns(new ContentfulCollection<Content>() { Items = new List<Content>() { content } });

        var model = new FeedbackModel()
        {
            Page = "foo"
        };

        // act
        var actual = await _feedbackController.JsonFeedback(model) as OkResult;

        // assert
        actual.Should().NotBeNull();
    }

    [TestCase("!")]
    [TestCase("foo.")]
    [TestCase("_foo")]
    [TestCase("some-fo:o")]
    [TestCase("'some-foo'")]
    [TestCase("1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890")]
    public async Task Feedback_Rejects_Invalid_PageId(string pageId)
    {
        // arrange
        var model = new FeedbackModel { Page = pageId };

        // act
        var actual = await _feedbackController.Feedback(model) as BadRequestResult;

        // assert
        actual.Should().NotBeNull();
    }

    [TestCase("!")]
    [TestCase("foo.")]
    [TestCase("_foo")]
    [TestCase("some-fo:o")]
    [TestCase("'some-foo'")]
    [TestCase("1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890")]
    public async Task JsonFeedback_Rejects_Invalid_PageId(string pageId)
    {
        // arrange
        var model = new FeedbackModel { Page = pageId };

        // act
        var actual = await _feedbackController.JsonFeedback(model) as BadRequestResult;

        // assert
        actual.Should().NotBeNull();
    }

    [Test]
    public async Task Feedback_Rejects_Invalid_Comments()
    {
        // arrange
        var model = new FeedbackModel { Comments = new string('a', 501) };

        // act
        var actual = await _feedbackController.Feedback(model) as BadRequestResult;

        // assert
        actual.Should().NotBeNull();
    }

    [Test]
    public async Task JsonFeedback_Rejects_Invalid_Comments()
    {
        // arrange
        var model = new FeedbackModel { Comments = new string('a', 501) };

        // act
        var actual = await _feedbackController.JsonFeedback(model) as BadRequestResult;

        // assert
        actual.Should().NotBeNull();
    }
}