using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.GraphQL.Queries;
using GraphQL.Client.Abstractions.Websocket;

namespace Childrens_Social_Care_CPD.DataAccess
{
    public interface IRolesRepository
    {
        Task<Content> GetByIdAsync(string id, int depth = 10, CancellationToken cancellationToken = default);
    }
    public class RolesRepository : IRolesRepository
    {
        private readonly IGraphQLWebSocketClient _gqlClient;
        private readonly bool _isPreview;
        public RolesRepository(IApplicationConfiguration applicationConfiguration, IGraphQLWebSocketClient gqlClient) 
        {
            _gqlClient = gqlClient;
            _isPreview = ContentfulConfiguration.IsPreviewEnabled(applicationConfiguration);
        }

        public async Task<Content> GetByIdAsync(string id, int depth = 10, CancellationToken cancellationToken = default)
        {

            var result = await _gqlClient
                .SendQueryAsync<GetRoles.ResponseType>(GetRoles.Query(id, _isPreview), cancellationToken);

            var first = result.Data.ContentCollection.Items.FirstOrDefault(); //.ContentfulMetaData.Tags.Where(x => x.Name.StartsWith($"{prefix}:"));

            if (first == null)
            {
                return new Content();
            }

            //var roles = first.Items.Select(x => new RoleItem
            //{
            //    Title = x.Title,

            //});

            //List<IContent> roleList = new List<IContent>();
            //foreach(var item in roles)
            //{
            //    roleList.Add(new RoleList { Title = item.Title });
            //}

            return new Content
            {
                Id = first.Id,
                Title = first.Title,
                Category = first.Category,
                ContentTitle = first.ContentTitle,
                ContentSubtitle = first.ContentSubtitle,
                ShowContentHeader = first.ShowContentHeader,
                //Items = roleList
            };

        }
    }
}
