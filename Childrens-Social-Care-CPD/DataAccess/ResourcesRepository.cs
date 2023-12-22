using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Core.Resources;
using Childrens_Social_Care_CPD.GraphQL.Queries;
using Contentful.Core.Search;
using GraphQL.Client.Abstractions.Websocket;

namespace Childrens_Social_Care_CPD.DataAccess;

public interface IResourcesRepository
{
    Task<Content> FetchRootPageAsync(CancellationToken cancellationToken = default);
    Task<SearchResourcesByTags.ResponseType> FindByTagsAsync(IEnumerable<string> tags, int skip, int take, ResourceSortOrder resourceSortOrder, CancellationToken cancellationToken = default);
    Task<IEnumerable<TagInfo>> GetSearchTagsAsync(CancellationToken cancellationToken);
    Task<Tuple<Content, GetContentTags.ResponseType>> GetByIdAsync(string id, int depth = 10, CancellationToken cancellationToken = default);
}

public class ResourcesRepository : IResourcesRepository
{
    private static readonly string[] _tagPrefixes = new string[] { "Topic", "Resource provider", "Format", "Career stage" };
    private readonly ICpdContentfulClient _cpdClient;
    private readonly IGraphQLWebSocketClient _gqlClient;
    private readonly bool _isPreview;

    public ResourcesRepository(IApplicationConfiguration applicationConfiguration, ICpdContentfulClient cpdClient, IGraphQLWebSocketClient gqlClient)
    {
        _cpdClient = cpdClient;
        _gqlClient = gqlClient;
        _isPreview = ContentfulConfiguration.IsPreviewEnabled(applicationConfiguration);
    }

    public Task<Content> FetchRootPageAsync(CancellationToken cancellationToken = default)
    {
        var queryBuilder = QueryBuilder<Content>.New
            .ContentTypeIs("content")
            .Include(10)
            .FieldEquals("fields.id", "resources");

        return _cpdClient
            .GetEntries(queryBuilder, cancellationToken)
            .ContinueWith(x => x.Result.FirstOrDefault());
    }

    public Task<SearchResourcesByTags.ResponseType> FindByTagsAsync(IEnumerable<string> tags, int skip, int take, ResourceSortOrder resourceSortOrder, CancellationToken cancellationToken = default)
    {
        string order = (resourceSortOrder == ResourceSortOrder.UpdatedNewest) ? "sys_publishedAt_DESC" : "sys_publishedAt_ASC";

        return _gqlClient
            .SendQueryAsync<SearchResourcesByTags.ResponseType>(SearchResourcesByTags.Query(tags, take, skip, order, _isPreview), cancellationToken)
            .ContinueWith(x => x.Result.Data);
    }

    public async Task<Tuple<Content, GetContentTags.ResponseType>> GetByIdAsync(string id, int depth = 10, CancellationToken cancellationToken = default)
    {
        var queryBuilder = QueryBuilder<Content>.New
            .ContentTypeIs("content")
            .Include(depth)
            .FieldEquals("fields.id", id);

        var tagsTask = _gqlClient
            .SendQueryAsync<GetContentTags.ResponseType>(GetContentTags.Query(id, _isPreview), cancellationToken)
            .ContinueWith(x => x.Result.Data);

        var contentTask = _cpdClient
            .GetEntries(queryBuilder, cancellationToken)
            .ContinueWith(x => x.Result.FirstOrDefault());

        await Task.WhenAll(contentTask, tagsTask);
        return Tuple.Create(contentTask.Result, tagsTask.Result);
    }

    public async Task<IEnumerable<TagInfo>> GetSearchTagsAsync(CancellationToken cancellationToken = default)
    {
        var allTags = await _cpdClient.GetTags(string.Empty, cancellationToken);

        var tags = allTags
            .Where(x => Array.Exists(_tagPrefixes, prefix => x.Name.StartsWith($"{prefix}:")))
            .Select(x =>
            {
                var i = x.Name.IndexOf(':');
                var category = x.Name[..i];
                return KeyValuePair.Create(category, x);
            });

        var list = new List<TagInfo>();

        foreach (var category in _tagPrefixes)
        {
            list.AddRange(
                tags
                    .Where(x => x.Key == category)
                    .Select(x => new TagInfo(x.Key, x.Value.Name[(x.Value.Name.IndexOf(':') + 1)..], x.Value.SystemProperties.Id))
                    .OrderBy(x => x.TagName)
            );
        }

        return list;
    }
}
