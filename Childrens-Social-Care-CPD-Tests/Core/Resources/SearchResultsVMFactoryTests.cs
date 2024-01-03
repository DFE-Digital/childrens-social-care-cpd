﻿using Azure.Search.Documents.Models;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Core.Resources;
using Childrens_Social_Care_CPD.DataAccess;
using Childrens_Social_Care_CPD.Models;
using Childrens_Social_Care_CPD.Search;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.Core.Resources;

public class SearchResultsVMFactoryTests
{
    private ISearchService _searchService;
    private IResourcesRepository _resourcesRepository;
    private ISearchResultsVMFactory _searchResultsVMFactory;

    private readonly SearchResults<CpdDocument> _emptySearchResults = SearchModelFactory.SearchResults(Array.Empty<SearchResult<CpdDocument>>(), 0, new Dictionary<string, IList<FacetResult>>(), 0, null);

    [SetUp]
    public void Setup()
    {
        _searchService = Substitute.For<ISearchService>();
        _resourcesRepository = Substitute.For<IResourcesRepository>();
        _searchResultsVMFactory = new SearchResultsVMFactory(_searchService, _resourcesRepository);
    }

    private static SearchResourcesResult GenerateSearchResults(int count, IDictionary<string, IList<FacetResult>> facets = null)
    {
        var random = new Random();
        List<SearchResult<CpdDocument>> results = new List<SearchResult<CpdDocument>>();
        for (var i = 0; i < count; i++)
        {
            results.Add(SearchModelFactory.SearchResult(new CpdDocument(), random.NextDouble(), new Dictionary<string, IList<string>>()));
        }

        var searchResults = SearchModelFactory.SearchResults(results, count, facets ?? new Dictionary<string, IList<FacetResult>>(), 0, null);
        return new SearchResourcesResult(count, count, 1, 1, 1, searchResults);
    }

    [TestCase(SortOrder.MostRelevant, SortCategory.Relevancy, SortDirection.Descending)]
    [TestCase(SortOrder.UpdatedLatest, SortCategory.Updated, SortDirection.Descending)]
    [TestCase(SortOrder.UpdatedOldest, SortCategory.Updated, SortDirection.Ascending)]
    public async Task GetSearchModel_Converts_SortOrder(SortOrder sortOrder, SortCategory sortCategory, SortDirection sortDirection)
    {
        // arrange
        _resourcesRepository.GetSearchTagsAsync(Arg.Any<CancellationToken>()).Returns(Array.Empty<TagInfo>());
        _resourcesRepository.FetchRootPageAsync().Returns(Task.FromResult<Content>(null));

        KeywordSearchQuery query = null;
        _searchService.SearchResourcesAsync(Arg.Do<KeywordSearchQuery>(x => query = x)).Returns(new SearchResourcesResult(0, 0, 0, 0, 0, _emptySearchResults));
        var request = new SearchRequestModel(string.Empty, Array.Empty<string>(), 1, sortOrder);

        // act
        var result = await _searchResultsVMFactory.GetSearchModel(request, 1, string.Empty, default);

        // assert
        query.SortCategory.Should().Be(sortCategory);
        query.SortDirection.Should().Be(sortDirection);
    }

    [Test]
    public async Task GetSearchModel_Passes_Term()
    {
        // arrange
        _resourcesRepository.GetSearchTagsAsync(Arg.Any<CancellationToken>()).Returns(Array.Empty<TagInfo>());
        _resourcesRepository.FetchRootPageAsync().Returns(Task.FromResult<Content>(null));

        KeywordSearchQuery query = null;
        _searchService.SearchResourcesAsync(Arg.Do<KeywordSearchQuery>(x => query = x)).Returns(GenerateSearchResults(50));
        var request = new SearchRequestModel("foo", Array.Empty<string>(), 1, SortOrder.MostRelevant);

        // act
        var result = await _searchResultsVMFactory.GetSearchModel(request, 1, string.Empty, default);

        // assert
        query.Keyword.Should().Be(request.Term);
    }

    [Test]
    public async Task GetSearchModel_Model_Should_Receive_SearchResults()
    {
        // arrange
        var count = 50;
        _resourcesRepository.GetSearchTagsAsync(Arg.Any<CancellationToken>()).Returns(Array.Empty<TagInfo>());
        _resourcesRepository.FetchRootPageAsync().Returns(Task.FromResult<Content>(null));

        _searchService.SearchResourcesAsync(Arg.Any<KeywordSearchQuery>()).Returns(GenerateSearchResults(count));
        var request = new SearchRequestModel(string.Empty, Array.Empty<string>(), 1, SortOrder.MostRelevant);

        // act
        var result = await _searchResultsVMFactory.GetSearchModel(request, 1, string.Empty, default);

        // assert
        result.TotalCount.Should().Be(count);
        result.TotalPages.Should().Be(count);
        result.CurrentPage.Should().Be(1);
        result.StartResultCount.Should().Be(1);
        result.EndResultCount.Should().Be(1);
        result.SearchResults.Should().HaveCount(count);
    }

    [Test]
    public async Task GetSearchModel_Model_Should_Receive_PageContent()
    {
        // arrange
        var content = new Content();
        _resourcesRepository.GetSearchTagsAsync(Arg.Any<CancellationToken>()).Returns(Array.Empty<TagInfo>());
        _resourcesRepository.FetchRootPageAsync().Returns(Task.FromResult(content));

        _searchService.SearchResourcesAsync(Arg.Any<KeywordSearchQuery>()).Returns(GenerateSearchResults(0));
        var request = new SearchRequestModel(string.Empty, Array.Empty<string>(), 1, SortOrder.MostRelevant);

        // act
        var result = await _searchResultsVMFactory.GetSearchModel(request, 1, string.Empty, default);

        // assert
        result.PageContent.Should().Be(content);
    }

    [Test]
    public async Task GetSearchModel_Model_Should_Receive_SearchTerm()
    {
        // arrange
        var term = "foo";
        _resourcesRepository.GetSearchTagsAsync(Arg.Any<CancellationToken>()).Returns(Array.Empty<TagInfo>());
        _resourcesRepository.FetchRootPageAsync().Returns(Task.FromResult<Content>(null));

        _searchService.SearchResourcesAsync(Arg.Any<KeywordSearchQuery>()).Returns(GenerateSearchResults(0));
        var request = new SearchRequestModel(term, Array.Empty<string>(), 1, SortOrder.MostRelevant);

        // act
        var result = await _searchResultsVMFactory.GetSearchModel(request, 1, string.Empty, default);

        // assert
        result.SearchTerm.Should().Be(term);
    }

    [Test]
    public async Task GetSearchModel_Model_Should_Receive_SortOrder()
    {
        // arrange
        var term = "foo";
        _resourcesRepository.GetSearchTagsAsync(Arg.Any<CancellationToken>()).Returns(Array.Empty<TagInfo>());
        _resourcesRepository.FetchRootPageAsync().Returns(Task.FromResult<Content>(null));

        _searchService.SearchResourcesAsync(Arg.Any<KeywordSearchQuery>()).Returns(GenerateSearchResults(0));
        var request = new SearchRequestModel(term, Array.Empty<string>(), 1, SortOrder.UpdatedOldest);

        // act
        var result = await _searchResultsVMFactory.GetSearchModel(request, 1, string.Empty, default);

        // assert
        result.SortOrder.Should().Be(SortOrder.UpdatedOldest);
    }

    [TestCase(SortOrder.UpdatedLatest, "/foo?term=foo")]
    [TestCase(SortOrder.UpdatedOldest, "/foo?term=foo&sortOrder=1")]
    [TestCase(SortOrder.MostRelevant, "/foo?term=foo&sortOrder=2")]
    public async Task GetSearchModel_Model_Should_Receive_ClearFiltersUri(SortOrder sortOrder, string expected)
    {
        // arrange
        var term = "foo";
        var routeName = "foo";
        _resourcesRepository.GetSearchTagsAsync(Arg.Any<CancellationToken>()).Returns(Array.Empty<TagInfo>());
        _resourcesRepository.FetchRootPageAsync().Returns(Task.FromResult<Content>(null));

        _searchService.SearchResourcesAsync(Arg.Any<KeywordSearchQuery>()).Returns(GenerateSearchResults(0));
        var request = new SearchRequestModel(term, Array.Empty<string>(), 1, sortOrder);

        // act
        var result = await _searchResultsVMFactory.GetSearchModel(request, 1, routeName, default);

        // assert
        result.ClearFiltersUri.Should().Be(expected);
    }

    [TestCase(SortOrder.UpdatedLatest, "/foo?page={0}&term=foo")]
    [TestCase(SortOrder.UpdatedOldest, "/foo?page={0}&sortOrder=1&term=foo")]
    [TestCase(SortOrder.MostRelevant, "/foo?page={0}&sortOrder=2&term=foo")]
    public async Task GetSearchModel_Model_Should_Receive_PagingFormatString(SortOrder sortOrder, string expected)
    {
        // arrange
        var term = "foo";
        var routeName = "foo";
        _resourcesRepository.GetSearchTagsAsync(Arg.Any<CancellationToken>()).Returns(Array.Empty<TagInfo>());
        _resourcesRepository.FetchRootPageAsync().Returns(Task.FromResult<Content>(null));

        _searchService.SearchResourcesAsync(Arg.Any<KeywordSearchQuery>()).Returns(GenerateSearchResults(0));
        var request = new SearchRequestModel(term, Array.Empty<string>(), 1, sortOrder);

        // act
        var result = await _searchResultsVMFactory.GetSearchModel(request, 1, routeName, default);

        // assert
        result.PagingFormatString.Should().Be(expected);
    }

    [Test]
    public async Task GetSearchModel_Model_Should_Receive_PagingFormatString_With_Tags()
    {
        // arrange
        var term = "foo";
        var routeName = "foo";
        var tagInfos = new[]
        {
            new TagInfo("Category1", "Tag One", "tagOne"),
            new TagInfo("Category1", "Tag Two", "tagTwo"),
        };

        _resourcesRepository.GetSearchTagsAsync(Arg.Any<CancellationToken>()).Returns(tagInfos);
        _resourcesRepository.FetchRootPageAsync().Returns(Task.FromResult<Content>(null));

        _searchService.SearchResourcesAsync(Arg.Any<KeywordSearchQuery>()).Returns(GenerateSearchResults(0));
        var request = new SearchRequestModel(term, new[] { "tagOne" } , 1, SortOrder.UpdatedLatest);

        // act
        var result = await _searchResultsVMFactory.GetSearchModel(request, 1, routeName, default);

        // assert
        result.PagingFormatString.Should().Be($"/foo?page={{0}}&term={term}&tags=tagOne");
    }

    [Test]
    public async Task GetSearchModel_Model_Should_Receive_Tags()
    {
        // arrange
        var random = new Random();
        var term = "foo";
        var routeName = "foo";
        var tagInfos = new[]
        {
            new TagInfo("Category1", "Tag One", "tagOne"),
            new TagInfo("Category1", "Tag Two", "tagTwo"),
        };

        var facet = SearchModelFactory.FacetResult(random.Next(100), new Dictionary<string, object> { { "value", "tagOne" } });
        var facets = new Dictionary<string, IList<FacetResult>> { { "Tags", new List<FacetResult> { facet } } };
        var searchResults = GenerateSearchResults(0, facets);

        _resourcesRepository.GetSearchTagsAsync(Arg.Any<CancellationToken>()).Returns(tagInfos);
        _resourcesRepository.FetchRootPageAsync().Returns(Task.FromResult<Content>(null));

        _searchService.SearchResourcesAsync(Arg.Any<KeywordSearchQuery>()).Returns(searchResults);
        var request = new SearchRequestModel(term, new[] { "tagOne" }, 1, SortOrder.UpdatedLatest);

        // act
        var result = await _searchResultsVMFactory.GetSearchModel(request, 1, routeName, default);

        // assert
        result.Tags.Should().BeSameAs(tagInfos);
        result.SelectedTags.Should().BeEquivalentTo(new[] { "tagOne" });
        result.FacetedTags.Should().HaveCount(2);
        result.FacetedTags.First().Value.Should().BeSameAs(facet);
        result.FacetedTags.Last().Value.Should().BeNull();
    }
}