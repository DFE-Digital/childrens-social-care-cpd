using Childrens_Social_Care_CPD.Controllers;
using Childrens_Social_Care_CPD.Models;

namespace Childrens_Social_Care_CPD.Core.Resources;

public interface IResourcesSearchStrategy
{
    Task<ResourcesListViewModel> SearchAsync(ResourcesQuery query, CancellationToken cancellationToken = default);
}
