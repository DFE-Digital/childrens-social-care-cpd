using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Core.Resources;
using Childrens_Social_Care_CPD.DataAccess;
using NSubstitute;
using NUnit.Framework;
using static Childrens_Social_Care_CPD.GraphQL.Queries.SearchResourcesByTags;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;

namespace Childrens_Social_Care_CPD_Tests.Core.Resources;

public class ResourcesDynamicTagsSearchStategyTests
{
    private IResourcesRepository _resourcesRepository;
    
    private IResourcesSearchStrategy _sut;

    private void SetTags(IEnumerable<TagInfo> tags)
    {
        _resourcesRepository.GetSearchTagsAsync().Returns(tags);
    }

    private void SetPageContent(Content content)
    {
        _resourcesRepository.FetchRootPageAsync(Arg.Any<CancellationToken>()).Returns(content);
    }

    private void SetSearchResults(ResponseType content)
    {
        _resourcesRepository
            .FindByTagsAsync(Arg.Any<IEnumerable<string>>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<ResourceSortOrder>(), Arg.Any<CancellationToken>())
            .Returns(content);
    }

    [SetUp]
    public void SetUp()
    {
        _resourcesRepository = Substitute.For<IResourcesRepository>();
        SetTags(new List<TagInfo>
        {
            new TagInfo("Cat1", "Tag1", "tag1")
        });
        _sut = new ResourcesDynamicTagsSearchStategy(_resourcesRepository);
    }

    [Test]
    public async Task Search_With_Empty_Query()
    {
        // act
        var actual = await _sut.SearchAsync(query: null);

        // assert
        actual.TotalPages.Should().Be(0);
        actual.TotalResults.Should().Be(0);
        actual.CurrentPage.Should().Be(0);
        actual.SelectedTags.Should().BeEmpty();
        actual.Results.Should().BeNull();
        actual.Content.Should().BeNull();
    }

    [Test]
    public async Task Search_Returns_Tags()
    {
        // act
        var actual = await _sut.SearchAsync(query: null);

        // assert
        actual.TagInfos.Should().HaveCount(1);
    }

    [Test]
    public async Task Search_Returns_CMS_Content()
    {
        // arrange
        var content = new Content();
        SetPageContent(content);

        // act
        var actual = await _sut.SearchAsync(query: null);

        // assert
        actual.Content.Should().Be(content);
    }

    [Test]
    public async Task Search_Selected_Tags_Are_Passed_Into_View()
    {
        // arrange
        var query = new ResourcesQuery
        {
            Page = 1,
            Tags = new string[] { "tag1", "tag2" }
        };

        // act
        var actual = await _sut.SearchAsync(query);

        // assert
        actual.SelectedTags.Should().Equal(query.Tags);
    }

    [Test]
    public async Task Search_Page_Set_To_Be_In_Bounds()
    {
        // arrange
        var results = new ResponseType()
        {
            ContentCollection = new ContentCollection()
            {
                Total = 3,
                Items = new Collection<SearchResult>()
                {
                    new SearchResult(),
                    new SearchResult(),
                    new SearchResult(),
                }
            }
        };
        SetSearchResults(results);
        var query = new ResourcesQuery
        {
            Page = 2,
            Tags = new string[] { "tag1", "tag2" }
        };

        // act
        var actual = await _sut.SearchAsync(query);

        // assert
        actual.CurrentPage.Should().Be(1);
    }

    [TestCase("")]
    [TestCase("-1")]
    [TestCase("x")]
    public async Task Invalid_Tags_Are_Not_Queried_For(string value)
    {
        // arrange
        IEnumerable<string> passedTags = null;
        var tags = new string[] { value, "tag1" };
        var query = new ResourcesQuery
        {
            Page = 2,
            Tags = tags
        };
        await _resourcesRepository.FindByTagsAsync(Arg.Do<IEnumerable<string>>(value => passedTags = value), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<ResourceSortOrder>(), Arg.Any<CancellationToken>());

        // act
        await _sut.SearchAsync(query);

        //assert
        passedTags.Should().NotContain(value);
    }

    [TestCase("")]
    [TestCase("x")]
    public async Task Invalid_Tags_Are_Sanitised(string value)
    {
        // arrange
        var tags = new string[] { value, "tag1" };
        var query = new ResourcesQuery
        {
            Page = 2,
            Tags = tags
        };

        // act
        var actual = await _sut.SearchAsync(query);

        //assert
        actual.SelectedTags.Should().NotContain(value);
    }

    [TestCase(ResourceSortOrder.UpdatedNewest)]
    [TestCase(ResourceSortOrder.UpdatedOldest)]
    public async Task SearchAsync_Paging_Should_Respect_SortOrder(ResourceSortOrder sortOrder)
    {
        // arrange
        var results = new ResponseType()
        {
            ContentCollection = new ContentCollection()
            {
                Total = 11,
                Items = new Collection<SearchResult>()
                {
                    new SearchResult(),
                    new SearchResult(),
                    new SearchResult(),
                    new SearchResult(),
                    new SearchResult(),
                    new SearchResult(),
                    new SearchResult(),
                    new SearchResult(),
                    new SearchResult(),
                    new SearchResult(),
                    new SearchResult(),
                }
            }
        };
        SetSearchResults(results);
        var query = new ResourcesQuery
        {
            Page = 2,
            Tags = new string[] { "tag1" },
            SortOrder = sortOrder
        };

        // act
        var actual = await _sut.SearchAsync(query);

        // assert
        actual.PagingFormatString.Should().Be($"/resources-learning?page={{0}}&sortOrder={(int)sortOrder}&tags=tag1");
    }
}
