using Contentful.Core.Models;
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
                      title
                      category
                      contentTitle
                      contentSubtitle
                      showContentHeader
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
            public ContentCollectionEx ContentCollection { get; set; }
        }

        public class ContentCollectionEx
        {
            [JsonPropertyName("items")]
            public ICollection<MainRoleContent> Items { get; set; }
            public int Total { get; set; }
        }

        public class MainRoleContent
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Category { get; set; }
            public string ContentTitle { get; set; }
            public string ContentSubtitle { get; set; }
            public string SearchSummary { get; set; }
            public bool ShowContentHeader { get; set; }

            [JsonPropertyName("items")]
            public ICollection<IContent> Items { get; set; }
            public PublishedInfoEx Sys { get; set; }
            public MetaDataEx ContentfulMetaData { get; set; }
        }

        //public class RoleItem
        //{
        //    public string Title { get; set; }
        //}

        public class PublishedInfoEx
        {
            public DateTime? PublishedAt { get; set; }
            public DateTime? FirstPublishedAt { get; set; }
        }

        public class MetaDataEx
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
