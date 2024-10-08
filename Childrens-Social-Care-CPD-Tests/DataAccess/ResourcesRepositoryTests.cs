﻿using Childrens_Social_Care_CPD.Configuration;
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
using System.Collections.ObjectModel;
using System;
using Contentful.Core.Models.Management;
using System.Linq;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.GraphQL.Queries;

namespace Childrens_Social_Care_CPD_Tests.DataAccess;

public class ResourcesRepositoryTests
{
    private CancellationTokenSource _cancellationTokenSource;
    private IApplicationConfiguration _applicationConfiguration;
    private ICpdContentfulClient _contentfulClient;
    private IGraphQLWebSocketClient _gqlClient;

    private void SetSearchResults(SearchResourcesByTags.ResponseType responseType)
    {
        var response = Substitute.For<GraphQLResponse<SearchResourcesByTags.ResponseType>>();
        response.Data = responseType;
        _gqlClient.SendQueryAsync<SearchResourcesByTags.ResponseType>(Arg.Any<GraphQLRequest>(), Arg.Any<CancellationToken>()).Returns(response);
    }

    [SetUp]
    public void Setup()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        _contentfulClient = Substitute.For<ICpdContentfulClient>();
        _gqlClient = Substitute.For<IGraphQLWebSocketClient>();

        // By default we want the preview flag set to false
        _applicationConfiguration.AzureEnvironment.Returns(ApplicationEnvironment.Development);
        _applicationConfiguration.ContentfulEnvironment.Returns(ApplicationEnvironment.Development);
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
        var results = new SearchResourcesByTags.ResponseType
        {
            ContentCollection = new SearchResourcesByTags.ContentCollection
            {
                Total = 1,
                Items = new Collection<SearchResourcesByTags.SearchResult>
                {
                    new()
                }
            }
        };
        SetSearchResults(results);
        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        var result = await sut.FindByTagsAsync(Array.Empty<string>(), 0, 1, ResourceSortOrder.UpdatedNewest, _cancellationTokenSource.Token);

        // assert
        result.Should().Be(results);
    }

    [Test]
    public async Task FindByTagsAsync_Limits_Results()
    {
        // arrange
        var results = new SearchResourcesByTags.ResponseType();
        var response = Substitute.For<GraphQLResponse<SearchResourcesByTags.ResponseType>>();
        response.Data = results;
        GraphQLRequest request = null;
        _gqlClient.SendQueryAsync<SearchResourcesByTags.ResponseType>(Arg.Do<GraphQLRequest>(value => request = value), Arg.Any<CancellationToken>()).Returns(response);

        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        await sut.FindByTagsAsync(Array.Empty<string>(), 0, 1, ResourceSortOrder.UpdatedNewest, _cancellationTokenSource.Token);

        // assert
        dynamic variables = request.Variables;
        (variables.limit as object).Should().Be(1);
    }

    [Test]
    public async Task FindByTagsAsync_Skips_Results()
    {
        // arrange
        var results = new SearchResourcesByTags.ResponseType();
        var response = Substitute.For<GraphQLResponse<SearchResourcesByTags.ResponseType>>();
        response.Data = results;
        GraphQLRequest request = null;
        _gqlClient.SendQueryAsync<SearchResourcesByTags.ResponseType>(Arg.Do<GraphQLRequest>(value => request = value), Arg.Any<CancellationToken>()).Returns(response);

        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        await sut.FindByTagsAsync(Array.Empty<string>(), 5, 1, ResourceSortOrder.UpdatedNewest, _cancellationTokenSource.Token);

        // assert
        dynamic variables = request.Variables;
        (variables.skip as object).Should().Be(5);
    }

    [Test]
    public async Task FindByTagsAsync_Preview_Flag_Is_False_By_Default()
    {
        // arrange
        var response = Substitute.For<GraphQLResponse<SearchResourcesByTags.ResponseType>>();
        response.Data = new SearchResourcesByTags.ResponseType();
        GraphQLRequest request = null;
        _gqlClient.SendQueryAsync<SearchResourcesByTags.ResponseType>(Arg.Do<GraphQLRequest>(value => request = value), Arg.Any<CancellationToken>()).Returns(response);

        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        await sut.FindByTagsAsync(Array.Empty<string>(), 5, 1, ResourceSortOrder.UpdatedNewest, _cancellationTokenSource.Token);

        // assert
        dynamic variables = request.Variables;
        (variables.preview as object).Should().Be(false);
    }

    [Test]
    public async Task FindByTagsAsync_Sets_Preview_Flag()
    {
        // arrange
        _applicationConfiguration.AzureEnvironment.Returns(ApplicationEnvironment.PreProduction);
        _applicationConfiguration.ContentfulEnvironment.Returns(ApplicationEnvironment.PreProduction);

        var response = Substitute.For<GraphQLResponse<SearchResourcesByTags.ResponseType>>();
        response.Data = new SearchResourcesByTags.ResponseType();
        GraphQLRequest request = null;
        _gqlClient.SendQueryAsync<SearchResourcesByTags.ResponseType>(Arg.Do<GraphQLRequest>(value => request = value), Arg.Any<CancellationToken>()).Returns(response);

        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        await sut.FindByTagsAsync(Array.Empty<string>(), 5, 1, ResourceSortOrder.UpdatedNewest, _cancellationTokenSource.Token);

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
            new() { Name = "Foo", SystemProperties = new SystemProperties { Id = "foo" } },
            new() { Name = "Topic: Foo", SystemProperties = new SystemProperties { Id = "topicFoo" } },
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
            new() { Name = "Topic: Foo", SystemProperties = new SystemProperties { Id = "topicFoo" } },
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
            new() { Name = "Topic: Foo", SystemProperties = new SystemProperties { Id = "topicFoo" } },
            new() { Name = "Foo: Foo", SystemProperties = new SystemProperties { Id = "fooFoo" } },
        };
        _contentfulClient.GetTags().Returns(tags);
        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        var actual = await sut.GetSearchTagsAsync();

        // assert
        actual.Any(x => x.TagName == "fooFoo").Should().BeFalse();
    }

    [Test]
    public async Task GetByIdAsync_Queries_Tags_By_Id()
    {
        // arrange
        var collection = new ContentfulCollection<Content>
        {
            Items = new List<Content>()
        };
        _contentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), Arg.Any<CancellationToken>()).Returns(collection);

        var results = new GetContentTags.ResponseType();
        var response = Substitute.For<GraphQLResponse<GetContentTags.ResponseType>>();
        response.Data = results;
        GraphQLRequest request = null;
        _gqlClient.SendQueryAsync<GetContentTags.ResponseType>(Arg.Do<GraphQLRequest>(value => request = value), Arg.Any<CancellationToken>()).Returns(response);

        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        await sut.GetByIdAsync("foo", cancellationToken: _cancellationTokenSource.Token);

        // assert
        dynamic variables = request.Variables;
        (variables.id as object).Should().Be("foo");
    }

    [Test]
    public async Task GetByIdAsync_Preview_Flag_Is_False_By_Default()
    {
        // arrange
        var collection = new ContentfulCollection<Content>
        {
            Items = new List<Content>()
        };
        _contentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), Arg.Any<CancellationToken>()).Returns(collection);

        var results = new GetContentTags.ResponseType();
        var response = Substitute.For<GraphQLResponse<GetContentTags.ResponseType>>();
        response.Data = results;
        GraphQLRequest request = null;
        _gqlClient.SendQueryAsync<GetContentTags.ResponseType>(Arg.Do<GraphQLRequest>(value => request = value), Arg.Any<CancellationToken>()).Returns(response);

        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        await sut.GetByIdAsync("foo", cancellationToken: _cancellationTokenSource.Token);

        // assert
        dynamic variables = request.Variables;
        (variables.preview as object).Should().Be(false);
    }

    [Test]
    public async Task GetByIdAsync_Sets_Preview_Flag()
    {
        // arrange
        _applicationConfiguration.AzureEnvironment.Returns(ApplicationEnvironment.PreProduction);
        _applicationConfiguration.ContentfulEnvironment.Returns(ApplicationEnvironment.PreProduction);

        var collection = new ContentfulCollection<Content>
        {
            Items = new List<Content>()
        };
        _contentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), Arg.Any<CancellationToken>()).Returns(collection);

        var results = new GetContentTags.ResponseType();
        var response = Substitute.For<GraphQLResponse<GetContentTags.ResponseType>>();
        response.Data = results;
        GraphQLRequest request = null;
        _gqlClient.SendQueryAsync<GetContentTags.ResponseType>(Arg.Do<GraphQLRequest>(value => request = value), Arg.Any<CancellationToken>()).Returns(response);

        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        await sut.GetByIdAsync("foo", cancellationToken: _cancellationTokenSource.Token);

        // assert
        dynamic variables = request.Variables;
        (variables.preview as object).Should().Be(true);
    }

    [Test]
    public async Task GetByIdAsync_Returns_Data()
    {
        // arrange
        _applicationConfiguration.ContentfulEnvironment.Returns(ApplicationEnvironment.PreProduction);

        var content = new Content();
        var collection = new ContentfulCollection<Content>
        {
            Items = new List<Content> { content }
        };
        _contentfulClient.GetEntries(Arg.Any<QueryBuilder<Content>>(), Arg.Any<CancellationToken>()).Returns(collection);

        var results = new GetContentTags.ResponseType();
        var response = Substitute.For<GraphQLResponse<GetContentTags.ResponseType>>();
        response.Data = results;
        GraphQLRequest request = null;
        _gqlClient.SendQueryAsync<GetContentTags.ResponseType>(Arg.Do<GraphQLRequest>(value => request = value), Arg.Any<CancellationToken>()).Returns(response);

        var sut = new ResourcesRepository(_applicationConfiguration, _contentfulClient, _gqlClient);

        // act
        (var actualContent, var actualTags) = await sut.GetByIdAsync("foo", cancellationToken: _cancellationTokenSource.Token);

        // assert
        actualContent.Should().Be(content);
        actualTags.Should().Be(results);
    }

}
