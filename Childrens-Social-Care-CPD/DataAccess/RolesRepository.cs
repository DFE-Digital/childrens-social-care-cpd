using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.GraphQL.Queries;
using Contentful.Core.Models;
using GraphQL.Client.Abstractions.Websocket;
using System.Collections;

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
                    .SendQueryAsync<GetRoles.ResponseType>(GetRoles.Query(id, _isPreview), cancellationToken)
                    .ContinueWith(x => x.Result);

            var first = result.Data.ContentCollection.Items.FirstOrDefault();
            if (first == null)
            {
                return new Content();
            }

            var roleListTask = _gqlClient
                .SendQueryAsync<GetRoles.ResponseType2>(GetRoles.GetRoleList(_isPreview), cancellationToken)
                .ContinueWith(x => x.Result);

            var detailsListTask = _gqlClient
                .SendQueryAsync<GetRoles.ResponseType3>(GetRoles.GetDetailedRoles(_isPreview), cancellationToken)
                .ContinueWith(x => x.Result);


            await Task.WhenAll(roleListTask, detailsListTask);

            var roleList = roleListTask.Result;
            var detailsList = detailsListTask.Result;

            Dictionary<string, DetailedRole> detailedRoles = new();
            foreach (var item in detailsList.Data.DetailedRoleCollection.Items)
            {

                foreach (var item2 in item.LinkedFrom.ContentCollection.Items)
                {
                    if (item2 != null)
                    {
                        detailedRoles.Add(item2.Id, new DetailedRole { Title = item.Title, SalaryRange = item.SalaryRange, RoleListSummary = item.RoleListSummary });
                    }
                }
            }

            List<IContent> roles = new();

            foreach (var item in roleList.Data.RoleListCollection.Items)
            {
                RoleList roleListItem = new()
                {
                    Title = item.Title,
                };
                foreach (var item2 in item.RolesCollection.Items)
                {
                    if (item2 == null)
                        continue;

                    if (detailedRoles.ContainsKey(item2.Id))
                    {
                        Content content2 = new Content { Id = item2.Id, Title = detailedRoles[item2.Id].Title, ContentType = detailedRoles[item2.Id].GetType().ToString() };
                        content2.Items = new();
                        content2.Items.Add(detailedRoles[item2.Id]);
                        if (roleListItem.Roles == null)
                        {
                            roleListItem.Roles = new();
                        }
                        roleListItem.Roles.Add(content2);
                    }

                }

                if (roles.Any())
                {
                    IContent separatorcontent = (IContent)new ContentSeparator();
                    roles.Add(separatorcontent);
                }

                IContent content = (IContent)roleListItem;
                roles.Add(content);
            }

            return new Content
            {
                Id = first.Id,
                Title = first.Title,
                Category = first.Category,
                ContentTitle = first.ContentTitle,
                ContentSubtitle = first.ContentSubtitle,
                ShowContentHeader = first.ShowContentHeader,
                Items = new List<IContent>(roles)
            };
        }
    }
}
