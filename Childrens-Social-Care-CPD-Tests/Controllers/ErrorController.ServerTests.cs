using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Controllers;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.Controllers;

public class ErrorControllerServerTests
{
    private CpdTestServerApplication _application;
    private HttpClient _httpClient;

    [SetUp]
    public void SetUp()
    {
        _application = new CpdTestServerApplication();
        _httpClient = _application.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Test]
    public async Task Non_Existant_Page_Will_Return_404()
    {
        // arrange
        var contentCollection = new ContentfulCollection<Content>() { Items = new List<Content>() };
        _application.CpdContentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), Arg.Any<CancellationToken>()).Returns(contentCollection);
        var url = "/does_not_exist";

        // act
        var response = await _httpClient.GetAsync(url);
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.RequestMessage.RequestUri.AbsolutePath.Should().Be(url);
    }

    [Test]
    public async Task Exception_Will_Return_500()
    {
        // arrange
        _application.CpdContentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), Arg.Any<CancellationToken>()).Throws(new Exception("Test exception"));
        var url = "/something";

        // act
        var response = await _httpClient.GetAsync(url);

        // assert

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        response.RequestMessage.RequestUri.AbsolutePath.Should().Be(url);
        
    }

    [Test]
    public async Task Exception_Will_Be_Logged()
    {
        // arrange
        var exception = new TestException();
        var logger = Substitute.For<ILogger<ErrorController>>();
        _application.LoggerFactory.CreateLogger<ErrorController>().Returns(logger);
        _application.CpdContentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), Arg.Any<CancellationToken>()).Throws(exception);
        var url = "/something";

        // act
        await _httpClient.GetAsync(url);

        // assert
        logger.Received(1).LogError(exception, "Unhandled exception occurred");
    }
}
