using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.DataAccess;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using static Childrens_Social_Care_CPD.GraphQL.Queries.SearchResourcesByTags;
using System.Collections.Generic;
using System.Threading;
using Childrens_Social_Care_CPD.Core.Resources;
using FluentAssertions;
using System.Threading.Tasks;
using Childrens_Social_Care_CPD.Controllers;
using System.Collections.ObjectModel;

namespace Childrens_Social_Care_CPD_Tests.Core.Resources;

public class ResourcesFixedTagsSearchStrategyTests
{
    private IResourcesRepository _resourcesRepository;
    private ILogger<ResourcesFixedTagsSearchStrategy> _logger;
    private IResourcesSearchStrategy _sut;

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
        _logger = Substitute.For<ILogger<ResourcesFixedTagsSearchStrategy>>();
        _resourcesRepository = Substitute.For<IResourcesRepository>();

        _sut = new ResourcesFixedTagsSearchStrategy(_resourcesRepository, _logger);
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
        actual.TagInfos.Should().HaveCount(7);
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
            Tags = new string[] { "1", "2" }
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
            ResourceCollection = new ResourceCollection()
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
            Tags = new string[] { "1", "2" }
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
        var tags = new string[] { value, "5" };
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
        var tags = new string[] { value, "5" };
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
}
