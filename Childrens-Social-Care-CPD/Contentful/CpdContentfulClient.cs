using Contentful.Core;
using Contentful.Core.Configuration;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Contentful.Core.Search;
using Newtonsoft.Json;

namespace Childrens_Social_Care_CPD.Contentful;

public class CpdContentfulClient : ICpdContentfulClient
{
    private readonly IContentfulClient _client;

    public CpdContentfulClient(IContentfulClient client, IContentTypeResolver contentTypeResolver)
    {
        _client = client;
        _client.ContentTypeResolver = contentTypeResolver;
    }

    public IContentTypeResolver ContentTypeResolver { get => _client.ContentTypeResolver; set => _client.ContentTypeResolver = value; }
    public JsonSerializerSettings SerializerSettings { get => _client.SerializerSettings; set => _client.SerializerSettings = value; }

    public JsonSerializer Serializer => _client.Serializer;

    public bool ResolveEntriesSelectively { get => _client.ResolveEntriesSelectively; set => _client.ResolveEntriesSelectively = value; }

    public bool IsPreviewClient => _client.IsPreviewClient;

    public Task<EmbargoedAssetKey> CreateEmbargoedAssetKey(DateTimeOffset timeOffset, CancellationToken cancellationToken = default)
    {
        return _client.CreateEmbargoedAssetKey(timeOffset, cancellationToken);
    }

    public Task<ContentfulResult<Asset>> GetAsset(string assetId, string etag, string queryString = null, CancellationToken cancellationToken = default)
    {
        return _client.GetAsset(assetId, etag, queryString, cancellationToken);
    }

    public Task<Asset> GetAsset(string assetId, QueryBuilder<Asset> queryBuilder, CancellationToken cancellationToken = default)
    {
        return _client.GetAsset(assetId, queryBuilder, cancellationToken);
    }

    public Task<Asset> GetAsset(string assetId, string queryString = null, CancellationToken cancellationToken = default)
    {
        return _client.GetAsset(assetId, queryString, cancellationToken);
    }

    public Task<ContentfulResult<ContentfulCollection<Asset>>> GetAssets(string etag, string queryString = null, CancellationToken cancellationToken = default)
    {
        return _client.GetAssets(etag, queryString, cancellationToken);
    }

    public Task<ContentfulCollection<Asset>> GetAssets(QueryBuilder<Asset> queryBuilder, CancellationToken cancellationToken = default)
    {
        return _client.GetAssets(queryBuilder, cancellationToken);
    }

    public Task<ContentfulCollection<Asset>> GetAssets(string queryString = null, CancellationToken cancellationToken = default)
    {
        return _client.GetAssets(queryString, cancellationToken);
    }

    public Task<ContentfulResult<ContentType>> GetContentType(string etag, string contentTypeId, CancellationToken cancellationToken = default)
    {
        return _client.GetContentType(etag, contentTypeId, cancellationToken);
    }

    public Task<ContentType> GetContentType(string contentTypeId, CancellationToken cancellationToken = default)
    {
        return _client.GetContentType(contentTypeId, cancellationToken);
    }

    public Task<ContentfulResult<IEnumerable<ContentType>>> GetContentTypes(string etag, string queryString, CancellationToken cancellationToken = default)
    {
        return _client.GetContentTypes(etag, queryString, cancellationToken);
    }

    public Task<IEnumerable<ContentType>> GetContentTypes(CancellationToken cancellationToken = default)
    {
        return _client.GetContentTypes(cancellationToken);
    }

    public Task<IEnumerable<ContentType>> GetContentTypes(string queryString, CancellationToken cancellationToken = default)
    {
        return _client.GetContentTypes(queryString, cancellationToken);
    }

    public Task<ContentfulCollection<T>> GetEntries<T>(QueryBuilder<T> queryBuilder, CancellationToken cancellationToken = default)
    {
        return _client.GetEntries(queryBuilder, cancellationToken);
    }

    public Task<ContentfulResult<ContentfulCollection<T>>> GetEntries<T>(string etag, string queryString = null, CancellationToken cancellationToken = default)
    {
        return _client.GetEntries<T>(etag, queryString, cancellationToken);
    }

    public Task<ContentfulCollection<T>> GetEntries<T>(string queryString = null, CancellationToken cancellationToken = default)
    {
        return _client.GetEntries<T>(queryString, cancellationToken);
    }

    public Task<ContentfulCollection<T>> GetEntriesByType<T>(string contentTypeId, QueryBuilder<T> queryBuilder = null, CancellationToken cancellationToken = default)
    {
        return _client.GetEntriesByType(contentTypeId, queryBuilder, cancellationToken);
    }

    public Task<string> GetEntriesRaw(string queryString = null, CancellationToken cancellationToken = default)
    {
        return _client.GetEntriesRaw(queryString, cancellationToken);
    }

    public Task<ContentfulResult<T>> GetEntry<T>(string entryId, string etag, string queryString = null, CancellationToken cancellationToken = default)
    {
        return _client.GetEntry<T>(entryId, etag, queryString, cancellationToken);
    }

    public Task<T> GetEntry<T>(string entryId, QueryBuilder<T> queryBuilder, CancellationToken cancellationToken = default)
    {
        return _client.GetEntry(entryId, queryBuilder, cancellationToken);
    }

    public Task<T> GetEntry<T>(string entryId, string queryString = null, CancellationToken cancellationToken = default)
    {
        return _client.GetEntry<T>(entryId, queryString, cancellationToken);
    }

    public Task<ContentfulResult<IEnumerable<Locale>>> GetLocales(string etag, CancellationToken cancellationToken = default)
    {
        return _client.GetLocales(etag, cancellationToken);
    }

    public Task<IEnumerable<Locale>> GetLocales(CancellationToken cancellationToken = default)
    {
        return _client.GetLocales(cancellationToken);
    }

    public Task<ContentfulResult<Space>> GetSpace(string etag, CancellationToken cancellationToken = default)
    {
        return _client.GetSpace(etag, cancellationToken);
    }

    public Task<Space> GetSpace(CancellationToken cancellationToken = default)
    {
        return _client.GetSpace(cancellationToken);
    }

    public Task<ContentTag> GetTag(string tagId, CancellationToken cancellationToken = default)
    {
        return _client.GetTag(tagId, cancellationToken);
    }

    public Task<IEnumerable<ContentTag>> GetTags(string queryString = "", CancellationToken cancellationToken = default)
    {
        return _client.GetTags(queryString, cancellationToken);
    }

    public Task<SyncResult> SyncInitial(SyncType syncType = SyncType.All, string contentTypeId = "", CancellationToken cancellationToken = default, int? limit = null)
    {
        return _client.SyncInitial(syncType, contentTypeId, cancellationToken, limit);
    }

    public Task<SyncResult> SyncInitialRecursive(SyncType syncType = SyncType.All, string contentTypeId = "", CancellationToken cancellationToken = default, int? limit = null)
    {
        return _client.SyncInitialRecursive(syncType, contentTypeId, cancellationToken, limit);
    }

    public Task<SyncResult> SyncNextResult(string nextSyncOrPageUrl, CancellationToken cancellationToken = default)
    {
        return _client.SyncNextResult(nextSyncOrPageUrl, cancellationToken);
    }
}
