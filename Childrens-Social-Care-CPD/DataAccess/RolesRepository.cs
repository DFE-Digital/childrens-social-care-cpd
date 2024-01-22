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
                .SendQueryAsync<GetContentTags.ResponseType>(GetRoles.Query(id, _isPreview), cancellationToken);
                //.ContinueWith(x => x.Result.Data);

            return new Content();

        }
    }
}
