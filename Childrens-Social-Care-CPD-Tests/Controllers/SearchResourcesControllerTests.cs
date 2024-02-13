using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Models;
using Childrens_Social_Care_CPD.Search;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.Controllers;

public class SearchResourcesControllerTests
{
    private class MockSearchResultsVMFactory : ISearchResultsVMFactory
    {
        public Task<ResourceSearchResultsViewModel> GetSearchModel(SearchRequestModel query, int pageSize, string searchRoute, CancellationToken cancellationToken)
        {
            return Task.FromResult(new ResourceSearchResultsViewModel(
                string.Empty,
                0,0,0,0,0,
                null,
                null,
                null,
                null,
                string.Empty,
                string.Empty,
                SortOrder.Relevance));
        }
    }

    private IFeaturesConfig _featuresConfig;
    private ISearchResultsVMFactory _searchResultsVMFactory;
    private SearchResourcesController _sut;

    [SetUp]
    public void Setup()
    {
        _featuresConfig = Substitute.For<IFeaturesConfig>();
        _searchResultsVMFactory = new MockSearchResultsVMFactory();
        _sut = new SearchResourcesController(_featuresConfig, _searchResultsVMFactory);
    }

    [Test]
    public void SearchResources_WhenFeatureIsDisabled_ReturnsNotFound()
    {
        // arrange
        _featuresConfig.IsEnabled(Features.ResourcesAndLearning).Returns(false);

        // act
        var result = _sut.SearchResources(new SearchRequestModel { Term = string.Empty, Tags = null });

        // assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public void SearchResources_ViewResult_ReceivesViewModel()
    {
        // arrange
        _featuresConfig.IsEnabled(Features.ResourcesAndLearning).Returns(true);

        // act
        var result = _sut.SearchResources(new SearchRequestModel { Term = string.Empty, Tags = null });

        // assert
        result.Result.Should().BeOfType<ViewResult>();
        result.Result.As<ViewResult>().Model.Should().BeOfType<ResourceSearchResultsViewModel>();
    }
}