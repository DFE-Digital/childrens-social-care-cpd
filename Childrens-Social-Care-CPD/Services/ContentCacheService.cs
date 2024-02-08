using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.DataAccess;
using Microsoft.Extensions.Caching.Memory;

namespace Childrens_Social_Care_CPD.Services;

public class ContentCacheService : IContentRepository
{
    private readonly IContentRepository _contentRepository;
    private readonly IMemoryCache _cache;
    private const int _cacheDurationMinutes = 15; //could be moved to config if required

    public ContentCacheService(IContentRepository contentRepository, IMemoryCache cache)
    {
        _contentRepository = contentRepository;
        _cache = cache;
    }

    public async Task<Content> FetchPageContentAsync(string contentId, CancellationToken cancellationToken)
    {
        if (_cache.TryGetValue(contentId, out Content content))
        {
            return content;
        }

        content = await _contentRepository.FetchPageContentAsync(contentId, cancellationToken);

        if (content != null)
        {
            _cache.Set(contentId, content, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheDurationMinutes)
            });
        }

        return content;
    }
}
