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
            query SearchResourcesByTags($searchTags: [String!], $limit: Int, $skip: Int, $order: [ResourceOrder], $preview: Boolean) {
              resourceCollection(where: {
                    contentfulMetadata: {
                          tags_exists: true
                          tags: {
                            id_contains_some: $searchTags
                          }
                        }
                  }, limit: $limit, skip: $skip, order: $order, preview: $preview) {
                total
                items {
                  title
                  from
                  searchSummary
                  type
                  sys {
                    publishedAt
                    firstPublishedAt
                  }
                  linkedFrom {
                    contentCollection {
                      items {
                        id
                      }
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
        [JsonPropertyName("resourceCollection")]
        public ResourceCollection ResourceCollection { get; set; }
    }

    public class ResourceCollection
    {
        [JsonPropertyName("items")]
        public ICollection<SearchResult> Items { get; set; }
        public int Total { get; set; }
    }

    public class SearchResult
    {
        public string Title { get; set; }
        public string From { get; set; }
        public string SearchSummary { get; set; }
        public ICollection<string> Type { get; set; }
        public PublishedInfo Sys { get; set; }
        public LinkedFromContentCollection LinkedFrom { get; set; }
    }

    public class PublishedInfo
    {
        public DateTime? PublishedAt { get; set; }
        public DateTime? FirstPublishedAt { get; set; }
    }

    public class LinkedFromContentCollection
    {
        public LinkedFrom ContentCollection { get; set; }
    }

    public class LinkedFrom
    {
        public ICollection<LinkedItem> Items { get; set; }
    }

    public class LinkedItem
    {
        public string Id { get; set; }
    }
}



