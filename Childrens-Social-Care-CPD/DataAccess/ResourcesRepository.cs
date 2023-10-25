using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.GraphQL.Queries;
using Contentful.Core.Search;
using GraphQL.Client.Abstractions.Websocket;

namespace Childrens_Social_Care_CPD.DataAccess;

public interface IResourcesRepository
{
    Task<Content> FetchRootPage(CancellationToken cancellationToken = default);
    Task<SearchResourcesByTags.ResponseType> FindByTags(IEnumerable<string> tags, int skip, int take, CancellationToken cancellationToken = default);
}

public class ResourcesRepository : IResourcesRepository
{
    private readonly ICpdContentfulClient _cpdClient;
    private readonly IGraphQLWebSocketClient _gqlClient;
    private readonly bool _isPreview;

    public ResourcesRepository(IApplicationConfiguration applicationConfiguration, ICpdContentfulClient cpdClient, IGraphQLWebSocketClient gqlClient)
    {
        _cpdClient = cpdClient;
        _gqlClient = gqlClient;
        _isPreview = !string.IsNullOrEmpty(applicationConfiguration.ContentfulPreviewId);
    }

    public Task<Content> FetchRootPage(CancellationToken cancellationToken = default)
    {
        var queryBuilder = QueryBuilder<Content>.New
            .ContentTypeIs("content")
            .Include(10)
            .FieldEquals("fields.id", "resources");

        return _cpdClient
            .GetEntries(queryBuilder, cancellationToken)
            .ContinueWith(x => x.Result.FirstOrDefault());
    }

    public Task<SearchResourcesByTags.ResponseType> FindByTags(IEnumerable<string> tags, int skip, int take, CancellationToken cancellationToken = default)
    {
        return _gqlClient
            .SendQueryAsync<SearchResourcesByTags.ResponseType>(SearchResourcesByTags.Query(tags, take, skip, _isPreview), cancellationToken)
            .ContinueWith(x => x.Result.Data);
    }
}
