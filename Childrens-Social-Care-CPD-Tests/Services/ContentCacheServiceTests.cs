using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.DataAccess;
using Childrens_Social_Care_CPD.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests.Services;

public class ContentCacheServiceTests
{
    private IContentRepository _contentRepository;
    private IMemoryCache _cache;

    [SetUp]
    public void SetUp()
    {
        _contentRepository = Substitute.For<IContentRepository>();
        _cache = Substitute.For<IMemoryCache>();
    }


    [Test]
    public async Task FetchPageContentAsync_Returns_Cached_Content()
    {
        // arrange
        _cache.TryGetValue("home", out Arg.Any<Content>()).Returns(true);
        _cache.TryGetValue("home", out Content content).Returns(true);

        // act
        var contentCacheService = new ContentCacheService(_contentRepository, _cache);
        var actual = await contentCacheService.FetchPageContentAsync("home", CancellationToken.None);

        // assert
        actual.Should().Be(content);
    }

    [Test]
    public async Task FetchPageContentAsync_Returns_NoneCached_Content()
    {
        // arrange
        _cache.TryGetValue("home", out Arg.Any<Content>()).Returns(false);
        _cache.TryGetValue("home", out Content content).Returns(false);

        // act
        var contentCacheService = new ContentCacheService(_contentRepository, _cache);
        var actual = await contentCacheService.FetchPageContentAsync("home", CancellationToken.None);

        // assert
        actual.Should().Be(content);
    }
}
