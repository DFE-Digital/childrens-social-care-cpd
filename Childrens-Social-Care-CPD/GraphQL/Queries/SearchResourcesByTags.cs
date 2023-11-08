using GraphQL;
using System.Text.Json.Serialization;

namespace Childrens_Social_Care_CPD.GraphQL.Queries;

public class SearchResourcesByTags
{
    public static GraphQLRequest Query(IEnumerable<string> tags, int limit, int skip, string order = "sys_publishedAt_ASC", bool preview = false)
    {
        return new GraphQLRequest
        {
            Query = @"
            query SearchResourcesByTags($searchTags: [String!], $limit: Int, $skip: Int, $order: [ContentOrder], $preview: Boolean) {
              contentCollection(where: {
                    contentType: ""Resource""
                    contentfulMetadata: {
                          tags_exists: true
                          tags: {
                            id_contains_some: $searchTags
                          }
                        }
                  }, limit: $limit, skip: $skip, order: $order, preview: $preview) {
                total
                items {
                  id
                  contentTitle
                  searchSummary
                  sys {
                    publishedAt
                    firstPublishedAt
                  }
                  contentfulMetadata {
                    tags {
                      id
                      name
                    }
                  }
                }
              }
            }",
            OperationName = "SearchResourcesByTags",
            Variables = new
            {
                searchTags = tags,
                limit,
                skip,
                order,
                preview,
            }
        };
    }

    public class ResponseType
    {
        [JsonPropertyName("contentCollection")]
        public ContentCollection ContentCollection { get; set; }
    }

    public class ContentCollection
    {
        [JsonPropertyName("items")]
        public ICollection<SearchResult> Items { get; set; }
        public int Total { get; set; }
    }

    public class SearchResult
    {
        public string Id { get; set; }
        public string ContentTitle { get; set; }
        public string SearchSummary { get; set; }
        public PublishedInfo Sys { get; set; }
        public MetaData ContentfulMetaData { get; set; }
    }

    public class PublishedInfo
    {
        public DateTime? PublishedAt { get; set; }
        public DateTime? FirstPublishedAt { get; set; }
    }

    public class MetaData
    {
        public List<Tag> Tags { get; set; }
    }

    public class Tag
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}