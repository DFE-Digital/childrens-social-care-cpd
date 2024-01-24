using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.DataAccess;
using Childrens_Social_Care_CPD.GraphQL.Queries;
using Contentful.Core.Models;
using GraphQL;
using GraphQL.Client.Abstractions.Websocket;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Childrens_Social_Care_CPD.GraphQL.Queries.GetRoles;

namespace Childrens_Social_Care_CPD_Tests.DataAccess;

public class RoleRepositoryTests
{
    private IApplicationConfiguration _applicationConfiguration;
    private IGraphQLWebSocketClient _gqlClient;

    private void SetContentCollection(GetRoles.ResponseType responseType)
    {
        var response = Substitute.For<GraphQLResponse<GetRoles.ResponseType>>();
        response.Data = responseType;
        _gqlClient.SendQueryAsync<GetRoles.ResponseType>(Arg.Any<GraphQLRequest>(), Arg.Any<CancellationToken>()).Returns(response);
    }

    private void SetRoleList(GetRoles.ResponseType2 responseType)
    {
        var response = Substitute.For<GraphQLResponse<GetRoles.ResponseType2>>();
        response.Data = responseType;
        _gqlClient.SendQueryAsync<GetRoles.ResponseType2>(Arg.Any<GraphQLRequest>(), Arg.Any<CancellationToken>()).Returns(response);
    }

    private void SetDetailsList(GetRoles.ResponseType3 responseType)
    {
        var response = Substitute.For<GraphQLResponse<GetRoles.ResponseType3>>();
        response.Data = responseType;
        _gqlClient.SendQueryAsync<GetRoles.ResponseType3>(Arg.Any<GraphQLRequest>(), Arg.Any<CancellationToken>()).Returns(response);
    }

    private Content GetExpectedResult()
    {
        List<IContent> roles = new();

        RoleList roleList1 = new()
        {
            Title = "Test Title",
            Roles = new List<Content>
            {
                new Content
                {
                    Id = "123",
                    Title = "Test Title"
                }
            }
        };

        RoleList roleList2 = new()
        {
            Title = "Test Title",
            Roles = new List<Content>
            {
                new Content
                {
                    Id = "123",
                    Title = "Test Title"
                }
            }
        };

        IContent content = (IContent)roleList1;
        roles.Add(content);
        content = (IContent)new ContentSeparator();
        roles.Add(content);
        content = (IContent)roleList2;
        roles.Add(content);

        return new Content
        {
            Id = "123",
            Title = "Test Title",
            Category = "Test Category",
            ContentTitle = "Test Content Title",
            ShowContentHeader = true,
            Items = new List<IContent>(roles)
        };
    }

    [SetUp]
    public void Setup()
    {
        _applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        _gqlClient = Substitute.For<IGraphQLWebSocketClient>();

        // By default we want the preview flag set to false
        _applicationConfiguration.AzureEnvironment.Returns(new StringConfigSetting(() => ApplicationEnvironment.Development));
        _applicationConfiguration.ContentfulEnvironment.Returns(new StringConfigSetting(() => ApplicationEnvironment.Development));
    }

    [Test]
    public async Task GetByIdAsync_ReturnsContent()
    {
        // Arrange
        var repType = new GetRoles.ResponseType
        {
            ContentCollection = new GetRoles.ContentCollectionEx
            {
                Items = new List<MainRoleContent>
                {
                    new MainRoleContent
                    {
                        Id = "123",
                        Title = "Test Title",
                        Category = "Test Category",
                        ContentTitle = "Test Content Title",
                        SearchSummary = "Test Search Summary",
                        ShowContentHeader = true,

                        Items = new List<IContent>(),
                        Sys = new PublishedInfoEx
                        {
                            PublishedAt = DateTime.Now,
                            FirstPublishedAt = DateTime.Now
                        },
                        ContentfulMetaData = new MetaDataEx
                        {
                            Tags = new List<Tag>
                            {
                                new Tag
                                {
                                    Id = "123",
                                    Name = "Test Name"
                                }
                            }
                        }
                    }
                }
            }
        };
        SetContentCollection(repType);
        var roleList = new GetRoles.ResponseType2
        {
            RoleListCollection = new RoleListCollection
            {
                Items = new List<RoleItems>
                {
                    new RoleItems
                    {
                        Title = "Test Title",
                        RolesCollection = new RolesCollection
                        {
                            Items = new List<RoleItem>
                            {
                                new RoleItem
                                {
                                    Id = "123"
                                }
                            }
                        }
                    },
                    new RoleItems
                    {
                        Title = "Test Title 2",
                        RolesCollection = new RolesCollection
                        {
                            Items = new List<RoleItem>
                            {
                                new RoleItem
                                {
                                    Id = "123"
                                }
                            }
                        }
                    }
                }
            }
        };
        SetRoleList(roleList);
        var detailsList = new GetRoles.ResponseType3
        {
            DetailedRoleCollection = new DetailedRoleCollection
            {
                Items = new List<DetailItem>
                {
                    new DetailItem
                    {
                        Title = "Test Title",
                        SalaryRange = "Test Salary Range",
                        RoleListSummary = "Test Role List Summary",
                        LinkedFrom = new LinkedFrom
                        {
                            ContentCollection = new ContentCollection
                            {
                                Items = new List<ContentItem>
                                {
                                    new ContentItem
                                    {
                                        Id = "123"
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };
        SetDetailsList(detailsList);
        var rolesRepository = new RolesRepository(_applicationConfiguration, _gqlClient);

        // Act
        var result = await rolesRepository.GetByIdAsync("123");

        // Assert
        GetExpectedResult().Should().BeEquivalentTo(result);
        
    }

    [Test]
    public async Task GetByIdAsync_ReturnsEmptyContent()
    {
        var expectedResults = new Content();
        var repType = new GetRoles.ResponseType()
        {
            ContentCollection = new GetRoles.ContentCollectionEx
            {
                Items = new List<MainRoleContent>()
            }
        };
        SetContentCollection(repType);
        var rolesRepository = new RolesRepository(_applicationConfiguration, _gqlClient);

        // Act
        var result = await rolesRepository.GetByIdAsync("123");

        // Assert
        expectedResults.Should().BeEquivalentTo(result);
    }
}
