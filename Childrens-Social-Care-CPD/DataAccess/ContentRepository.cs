using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Search;

namespace Childrens_Social_Care_CPD.DataAccess;

public interface IContentRepository
{
    Task<Content> FetchPageContentAsync(string contentId, CancellationToken cancellationToken);
}

public class ContentRepository : IContentRepository
{
    private readonly ICpdContentfulClient _cpdClient;
    public ContentRepository(ICpdContentfulClient cpdClient)
    {
        _cpdClient = cpdClient;
    }

    public async Task<Content> FetchPageContentAsync(string contentId, CancellationToken cancellationToken)
    {
        var queryBuilder = QueryBuilder<Content>.New
            .ContentTypeIs("content")
            .FieldEquals("fields.id", contentId)
            .Include(10);

        var result = await _cpdClient.GetEntries(queryBuilder, cancellationToken);

        return result?.FirstOrDefault();
    }
}
