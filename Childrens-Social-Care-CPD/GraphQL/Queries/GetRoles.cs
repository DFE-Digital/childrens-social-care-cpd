using GraphQL;
using System.Text.Json.Serialization;

namespace Childrens_Social_Care_CPD.GraphQL.Queries
{
    public static class GetRoles
    {
        public static GraphQLRequest Query(string id, bool preview = false)
        {
            return new GraphQLRequest
            {
                Query = @"
                query GetExploreRoles($id: String!, $preview: Boolean) {
                  contentCollection(where: {    
                        id : $id,
                      }, preview: $preview) {
                    total
                    items {
                      id
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
                OperationName = "GetExploreRoles",
                Variables = new
                {
                    id,
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
    
}
