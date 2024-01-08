using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Childrens_Social_Care_CPD.Search;
using Childrens_Social_Care_CPD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.Services;

public class SearchServiceTests
{
    private SearchClient _searchClient;
    private SearchService _sut;

    [SetUp]
    public void Setup()
    {
        _searchClient = Substitute.For<SearchClient>();
        _sut = new SearchService(_searchClient);
    }

    [Test]
    public async Task SearchResourcesAsync_Ignores_Empty_Filter()
    {
        // arrange
        SearchOptions options = null;
        var query = new KeywordSearchQuery("foo");
        var response = GenerateResponse();
        _searchClient.SearchAsync<CpdDocument>(Arg.Any<string>(), Arg.Do<SearchOptions>(x => options = x)).Returns(response);

        // act
        await _sut.SearchResourcesAsync(query);

        // assert
        options.Filter.Should().BeNull();
    }

    [Test]
    public async Task SearchResourcesAsync_Highlights_The_Body_Field()
    {
        // arrange
        SearchOptions options = null;
        var query = new KeywordSearchQuery("foo");
        var response = GenerateResponse();
        _searchClient.SearchAsync<CpdDocument>(Arg.Any<string>(), Arg.Do<SearchOptions>(x => options = x)).Returns(response);

        // act
        await _sut.SearchResourcesAsync(query);

        // assert
        options.HighlightFields.Should().NotBeNull();
        options.HighlightFields.Single().Should().Be("Body");
    }

    [Test]
    public async Task SearchResourcesAsync_Returns_Facets()
    {
        // arrange
        SearchOptions options = null;
        var query = new KeywordSearchQuery("foo");
        var response = GenerateResponse();
        _searchClient.SearchAsync<CpdDocument>(Arg.Any<string>(), Arg.Do<SearchOptions>(x => options = x)).Returns(response);

        // act
        await _sut.SearchResourcesAsync(query);

        // assert
        options.Facets.Should().NotBeNull();
        options.Facets.Single().Should().Be("Tags,count:100");
    }

    [Test]
    public async Task SearchResourcesAsync_Default_Ordering_Is_By_Relevancy_Descending()
    {
        // arrange
        SearchOptions options = null;
        var query = new KeywordSearchQuery("foo");
        var response = GenerateResponse();
        _searchClient.SearchAsync<CpdDocument>(Arg.Any<string>(), Arg.Do<SearchOptions>(x => options = x)).Returns(response);

        // act
        await _sut.SearchResourcesAsync(query);

        // assert
        options.OrderBy.Should().NotBeNull();
        options.OrderBy.Single().Should().Be("search.score() desc");
    }


    private static SearchResults<CpdDocument> GenerateSearchResults(int count, IDictionary<string, IList<FacetResult>> facets = null)
    {
        var random = new Random();
        var results = new List<SearchResult<CpdDocument>>();
        for (var i = 0; i < count; i++)
        {
            results.Add(SearchModelFactory.SearchResult(new CpdDocument(), random.NextDouble(), new Dictionary<string, IList<string>>()));
        }

        return SearchModelFactory.SearchResults(results, count, facets ?? new Dictionary<string, IList<FacetResult>>(), 0, null);
    }

    private static Response<SearchResults<CpdDocument>> GenerateResponse()
    {
        var searchResults = GenerateSearchResults(10);
        var sub = Substitute.For<Response<SearchResults<CpdDocument>>>();
        sub.Value.Returns(searchResults);

        return sub;
    }

    [Test]
    public async Task SearchResourcesAsync_Uses_Simple_Query_Parser()
    {
        // arrange
        SearchOptions options = null;
        var query = new KeywordSearchQuery("foo");
        var response = GenerateResponse();
        _searchClient.SearchAsync<CpdDocument>(Arg.Any<string>(), Arg.Do<SearchOptions>(x => options = x)).Returns(response);

        // act
        await _sut.SearchResourcesAsync(query);

        // assert
        options.QueryType.Should().Be(SearchQueryType.Simple);
    }

    [Test]
    public async Task SearchResourcesAsync_Turns_Search_Term_Into_Prefix_Query()
    {
        // arrange
        var searchTerm = string.Empty;
        var query = new KeywordSearchQuery("foo");
        var response = GenerateResponse();
        _searchClient.SearchAsync<CpdDocument>(Arg.Do<string>(x => searchTerm = x), Arg.Any<SearchOptions>()).Returns(response);

        // act
        await _sut.SearchResourcesAsync(query);

        // assert
        searchTerm.Should().Be("foo*");
    }

    [Test]
    public async Task SearchResourcesAsync_Requests_Total_Result_Count()
    {
        // arrange
        SearchOptions options = null;
        var query = new KeywordSearchQuery("foo");
        var response = GenerateResponse();
        _searchClient.SearchAsync<CpdDocument>(Arg.Any<string>(), Arg.Do<SearchOptions>(x => options = x)).Returns(response);

        // act
        await _sut.SearchResourcesAsync(query);

        // assert
        options.IncludeTotalCount.Should().Be(true);
    }

    [TestCase(-1, 0)]
    [TestCase(0, 0)]
    [TestCase(1, 0)]
    [TestCase(2, 8)]
    [TestCase(3, 16)]
    [TestCase(99, 784)]
    public async Task SearchResourcesAsync_Skips_By_PageSize(int page, int skip)
    {
        // arrange
        SearchOptions options = null;
        var query = new KeywordSearchQuery(string.Empty)
        {
            Page = page
        };
        var response = GenerateResponse();
        _searchClient.SearchAsync<CpdDocument>(Arg.Any<string>(), Arg.Do<SearchOptions>(x => options = x)).Returns(response);

        // act
        await _sut.SearchResourcesAsync(query);

        // assert
        options.Skip.Should().Be(skip);
    }

    [TestCase(-1, 8)]
    [TestCase(0, 8)]
    [TestCase(1, 8)]
    [TestCase(7, 8)]
    [TestCase(8, 8)]
    [TestCase(9, 9)]
    [TestCase(99, 99)]
    public async Task SearchResourcesAsync_Uses_Correct_Page_Size(int requestedPageSize, int actualPageSize)
    {
        // arrange
        SearchOptions options = null;
        var query = new KeywordSearchQuery("foo")
        {
            PageSize = requestedPageSize
        };
        var response = GenerateResponse();
        _searchClient.SearchAsync<CpdDocument>(Arg.Any<string>(), Arg.Do<SearchOptions>(x => options = x)).Returns(response);

        // act
        await _sut.SearchResourcesAsync(query);

        // assert
        options.Size.Should().Be(actualPageSize);
    }

    [Test]
    public async Task SearchResourcesAsync_Filters_Should_Be_Logically_ORed()
    {
        // arrange
        SearchOptions options = null;
        var query = new KeywordSearchQuery("foo")
        {
            Filter = new Dictionary<string, IEnumerable<string>>()
            {
                { 
                    "tag", new List<string>
                    {
                        "filter1",
                        "filter2",
                    } 
                }
            }
        };
        var response = GenerateResponse();
        _searchClient.SearchAsync<CpdDocument>(Arg.Any<string>(), Arg.Do<SearchOptions>(x => options = x)).Returns(response);

        // act
        await _sut.SearchResourcesAsync(query);

        // assert
        options.Filter.Should().Be("tag/any(v: v eq 'filter1') or tag/any(v: v eq 'filter2')");
    }

    [TestCase(SortCategory.Created, SortDirection.Ascending, "CreatedAt asc")]
    [TestCase(SortCategory.Created, SortDirection.Descending, "CreatedAt desc")]
    [TestCase(SortCategory.Updated, SortDirection.Ascending, "UpdatedAt asc")]
    [TestCase(SortCategory.Updated, SortDirection.Descending, "UpdatedAt desc")]
    [TestCase(SortCategory.Relevancy, SortDirection.Ascending, "search.score() asc")]
    [TestCase(SortCategory.Relevancy, SortDirection.Descending, "search.score() desc")]
    public async Task SearchResourcesAsync_Constructs_OrderBy_Correctly(SortCategory sortCategory, SortDirection sortDirection, string expectedOrderBy)
    {
        // arrange
        SearchOptions options = null;
        var query = new KeywordSearchQuery("foo")
        {
            SortCategory = sortCategory,
            SortDirection = sortDirection,
        };
        var response = GenerateResponse();
        _searchClient.SearchAsync<CpdDocument>(Arg.Any<string>(), Arg.Do<SearchOptions>(x => options = x)).Returns(response);

        // act
        await _sut.SearchResourcesAsync(query);

        // assert
        options.OrderBy.Single().Should().Be(expectedOrderBy);
    }
}