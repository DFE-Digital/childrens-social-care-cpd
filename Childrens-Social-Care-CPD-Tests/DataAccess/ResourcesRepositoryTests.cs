using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.DataAccess;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FluentAssertions;
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
using Contentful.Core.Models.Management;
using System.Linq;

namespace Childrens_Social_Care_CPD_Tests.DataAccess;

public class ResourcesRepositoryTests
{
    private CancellationTokenSource _cancellationTokenSource;
    private IApplicationConfiguration _applicationConfiguration;
    private ICpdContentfulClient _contentfulClient;
    private IGraphQLWebSocketClient _gqlClient;

    private void SetSearchResults(ResponseType responseType)
    {
        var response = Substitute.For<GraphQLResponse<ResponseType>>();
        response.Data = responseType;
        _gqlClient.SendQueryAsync<ResponseType>(Arg.Any<GraphQLRequest>(), Arg.Any<CancellationToken>()).Returns(response);
    }

    [SetUp]
    public void Setup()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        _contentfulClient = Substitute.For<ICpdContentfulClient>();
        _gqlClient = Substitute.For<IGraphQLWebSocketClient>();

        // By default we want the preview flag set to false
        _applicationConfiguration.AzureEnvironment.Returns(new StringConfigSetting(() => ApplicationEnvironment.Development));
        _applicationConfiguration.ContentfulEnvironment.Returns(new StringConfigSetting(() => ApplicationEnvironment.Development));
    }

    [Test]
    public async Task FetchRootPageAsync_Returns_Root_Page_Returned_When_Found()
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
        var result = await sut.FetchRootPageAsync(_cancellationTokenSource.Token);

        // assert
        result.Should().Be(content);
    }

    [Test]
    public async Task FetchRootPageAsync_Returns_Null_When_Root_Page_Not_Found()
    {
        // arrange
        var collection = new ContentfulCollection<Content>
        {
            Items = new List<Content>()
        };
        _contentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), Arg.Any<CancellationToken>()).Returns(collection);
        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        var result = await sut.FetchRootPageAsync(_cancellationTokenSource.Token);

        // assert
        result.Should().BeNull();
    }

    [Test]
    public async Task FindByTagsAsync_Returns_Results()
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
        var result = await sut.FindByTagsAsync(Array.Empty<string>(), 0, 1, _cancellationTokenSource.Token);

        // assert
        result.Should().Be(results);
    }

    [Test]
    public async Task FindByTagsAsync_Limits_Results()
    {
        // arrange
        var results = new ResponseType();
        var response = Substitute.For<GraphQLResponse<ResponseType>>();
        response.Data = results;
        GraphQLRequest request = null;
        _gqlClient.SendQueryAsync<ResponseType>(Arg.Do<GraphQLRequest>(value => request = value), Arg.Any<CancellationToken>()).Returns(response);

        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        await sut.FindByTagsAsync(Array.Empty<string>(), 0, 1, _cancellationTokenSource.Token);

        // assert
        dynamic variables = request.Variables;
        (variables.limit as object).Should().Be(1);
    }

    [Test]
    public async Task FindByTagsAsync_Skips_Results()
    {
        // arrange
        var results = new ResponseType();
        var response = Substitute.For<GraphQLResponse<ResponseType>>();
        response.Data = results;
        GraphQLRequest request = null;
        _gqlClient.SendQueryAsync<ResponseType>(Arg.Do<GraphQLRequest>(value => request = value), Arg.Any<CancellationToken>()).Returns(response);

        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        await sut.FindByTagsAsync(Array.Empty<string>(), 5, 1, _cancellationTokenSource.Token);

        // assert
        dynamic variables = request.Variables;
        (variables.skip as object).Should().Be(5);
    }

    [Test]
    public async Task FindByTagsAsync_Preview_Flag_Is_False_By_Default()
    {
        // arrange
        var response = Substitute.For<GraphQLResponse<ResponseType>>();
        response.Data = new ResponseType();
        GraphQLRequest request = null;
        _gqlClient.SendQueryAsync<ResponseType>(Arg.Do<GraphQLRequest>(value => request = value), Arg.Any<CancellationToken>()).Returns(response);

        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        await sut.FindByTagsAsync(Array.Empty<string>(), 5, 1, _cancellationTokenSource.Token);

        // assert
        dynamic variables = request.Variables;
        (variables.preview as object).Should().Be(false);
    }

    [Test]
    public async Task FindByTagsAsync_Sets_Preview_Flag()
    {
        // arrange
        _applicationConfiguration.ContentfulEnvironment.Returns(new StringConfigSetting(() => ApplicationEnvironment.PreProduction));

        var response = Substitute.For<GraphQLResponse<ResponseType>>();
        response.Data = new ResponseType();
        GraphQLRequest request = null;
        _gqlClient.SendQueryAsync<ResponseType>(Arg.Do<GraphQLRequest>(value => request = value), Arg.Any<CancellationToken>()).Returns(response);

        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        await sut.FindByTagsAsync(Array.Empty<string>(), 5, 1, _cancellationTokenSource.Token);

        // assert
        dynamic variables = request.Variables;
        (variables.preview as object).Should().Be(true);
    }

    [Test]
    public async Task GetSearchTagsAsync_Strips_Ungrouped_Tags()
    {
        // arrange
        var tags = new List<ContentTag>
        {
            new ContentTag { Name = "Foo", SystemProperties = new SystemProperties { Id = "foo" } },
            new ContentTag { Name = "Topic: Foo", SystemProperties = new SystemProperties { Id = "topicFoo" } },
        };
        _contentfulClient.GetTags().Returns(tags);
        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        var actual = await sut.GetSearchTagsAsync();

        // assert
        actual.Any(x => x.DisplayName == "Foo").Should().BeFalse();
    }

    [Test]
    public async Task GetSearchTagsAsync_Gets_Valid_Tags()
    {
        // arrange
        var tags = new List<ContentTag>
        {
            new ContentTag { Name = "Topic: Foo", SystemProperties = new SystemProperties { Id = "topicFoo" } },
        };
        _contentfulClient.GetTags().Returns(tags);
        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        var actual = await sut.GetSearchTagsAsync();

        // assert
        actual.Should().HaveCount(1);
        actual.First().TagName.Should().Be("topicFoo");
    }

    [Test]
    public async Task GetSearchTagsAsync_Strips_Unwanted_Grouped_Tags()
    {
        // arrange
        var tags = new List<ContentTag>
        {
            new ContentTag { Name = "Topic: Foo", SystemProperties = new SystemProperties { Id = "topicFoo" } },
            new ContentTag { Name = "Foo: Foo", SystemProperties = new SystemProperties { Id = "fooFoo" } },
        };
        _contentfulClient.GetTags().Returns(tags);
        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        var actual = await sut.GetSearchTagsAsync();

        // assert
        actual.Any(x => x.TagName == "fooFoo").Should().BeFalse();
    }
}
