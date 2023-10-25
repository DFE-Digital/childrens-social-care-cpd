using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.DataAccess;
using Childrens_Social_Care_CPD.GraphQL.Queries;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FluentAssertions;
using GraphQL.Client.Abstractions;
using GraphQL;
using GraphQL.Client.Abstractions.Websocket;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Childrens_Social_Care_CPD.GraphQL.Queries.SearchResourcesByTags;
using System.Collections.ObjectModel;
using System;
using Newtonsoft.Json.Linq;
using NSubstitute.Core;

namespace Childrens_Social_Care_CPD_Tests.DataAccess;

public class ResourcesRepositoryTests
{
    private CancellationTokenSource _cancellationTokenSource;
    private IApplicationConfiguration _applicationConfiguration;
    private ICpdContentfulClient _contentfulClient;
    private IGraphQLWebSocketClient _gqlClient;

    [SetUp]
    public void Setup()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        _contentfulClient = Substitute.For<ICpdContentfulClient>();
        _gqlClient = Substitute.For<IGraphQLWebSocketClient>();

        // By default we want the preview flag set to false
        _applicationConfiguration.ContentfulPreviewId.Returns(string.Empty);
    }

    [Test]
    public async Task FetchRootPage_Returns_Root_Page_Returned_When_Found()
    {
        // arrange
        var content = new Content();
        var collection = new ContentfulCollection<Content>
        {
            Items = new List<Content>
            {
                content
            }
        };
        _contentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), Arg.Any<CancellationToken>()).Returns(collection);
        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        var result = await sut.FetchRootPage(_cancellationTokenSource.Token);

        // assert
        result.Should().Be(content);
    }

    [Test]
    public async Task FetchRootPage_Returns_Null_When_Root_Page_Not_Found()
    {
        // arrange
        var collection = new ContentfulCollection<Content>
        {
            Items = new List<Content>()
        };
        _contentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), Arg.Any<CancellationToken>()).Returns(collection);
        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        var result = await sut.FetchRootPage(_cancellationTokenSource.Token);

        // assert
        result.Should().BeNull();
    }

    private void SetSearchResults(ResponseType responseType)
    {
        var response = Substitute.For<GraphQLResponse<ResponseType>>();
        response.Data = responseType;
        _gqlClient.SendQueryAsync<ResponseType>(Arg.Any<GraphQLRequest>(), Arg.Any<CancellationToken>()).Returns(response);
    }

    [Test]
    public async Task FindByTags_Returns_Results()
    {
        // arrange
        var results = new ResponseType
        {
            ResourceCollection = new ResourceCollection
            {
                Total = 1,
                Items = new Collection<SearchResult>
                {
                    new SearchResult()
                }
            }
        };
        SetSearchResults(results);
        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        var result = await sut.FindByTags(Array.Empty<string>(), 0, 1, _cancellationTokenSource.Token);

        // assert
        result.Should().Be(results);
    }

    [Test]
    public async Task FindByTags_Limits_Results()
    {
        // arrange
        var results = new ResponseType();
        var response = Substitute.For<GraphQLResponse<ResponseType>>();
        response.Data = results;
        GraphQLRequest request = null;
        _gqlClient.SendQueryAsync<ResponseType>(Arg.Do<GraphQLRequest>(value => request = value), Arg.Any<CancellationToken>()).Returns(response);

        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        await sut.FindByTags(Array.Empty<string>(), 0, 1, _cancellationTokenSource.Token);

        // assert
        dynamic variables = request.Variables;
        (variables.limit as object).Should().Be(1);
    }

    [Test]
    public async Task FindByTags_Skips_Results()
    {
        // arrange
        var results = new ResponseType();
        var response = Substitute.For<GraphQLResponse<ResponseType>>();
        response.Data = results;
        GraphQLRequest request = null;
        _gqlClient.SendQueryAsync<ResponseType>(Arg.Do<GraphQLRequest>(value => request = value), Arg.Any<CancellationToken>()).Returns(response);

        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        await sut.FindByTags(Array.Empty<string>(), 5, 1, _cancellationTokenSource.Token);

        // assert
        dynamic variables = request.Variables;
        (variables.skip as object).Should().Be(5);
    }

    [Test]
    public async Task FindByTags_Preview_Flag_Is_False_By_Default()
    {
        // arrange
        var response = Substitute.For<GraphQLResponse<ResponseType>>();
        response.Data = new ResponseType();
        GraphQLRequest request = null;
        _gqlClient.SendQueryAsync<ResponseType>(Arg.Do<GraphQLRequest>(value => request = value), Arg.Any<CancellationToken>()).Returns(response);

        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        await sut.FindByTags(Array.Empty<string>(), 5, 1, _cancellationTokenSource.Token);

        // assert
        dynamic variables = request.Variables;
        (variables.preview as object).Should().Be(false);
    }

    [Test]
    public async Task FindByTags_Sets_Preview_Flag()
    {
        // arrange
        _applicationConfiguration.ContentfulPreviewId.Returns("foo");

        var response = Substitute.For<GraphQLResponse<ResponseType>>();
        response.Data = new ResponseType();
        GraphQLRequest request = null;
        _gqlClient.SendQueryAsync<ResponseType>(Arg.Do<GraphQLRequest>(value => request = value), Arg.Any<CancellationToken>()).Returns(response);

        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        await sut.FindByTags(Array.Empty<string>(), 5, 1, _cancellationTokenSource.Token);

        // assert
        dynamic variables = request.Variables;
        (variables.preview as object).Should().Be(true);
    }
}
