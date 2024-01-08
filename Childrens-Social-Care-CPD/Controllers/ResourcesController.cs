using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.DataAccess;
using Childrens_Social_Care_CPD.GraphQL.Queries;
using Childrens_Social_Care_CPD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Childrens_Social_Care_CPD.Controllers;

public enum ResourceSortOrder
{ 
    UpdatedNewest = 0,
    UpdatedOldest = 1
}

public class ResourcesQuery
{
    public string[] Tags { get; set; }
    public int Page { get; set; } = 1;
    public ResourceSortOrder SortOrder { get; set; }

    public ResourcesQuery()
    {
        Tags = Array.Empty<string>();
    }
}

public class ResourcesController : Controller
{
    private readonly IFeaturesConfig _featuresConfig;
    private readonly IResourcesRepository _resourcesRepository;

    public ResourcesController(IFeaturesConfig featuresConfig, IResourcesRepository resourcesRepository)
    {
        _featuresConfig = featuresConfig;
        _resourcesRepository = resourcesRepository;
    }

    private Dictionary<string, string> GetProperties(GetContentTags.ResponseType tags, Content content)
    {
        var properties = new Dictionary<string, string>()
        {
            { "Published", content.Sys.CreatedAt?.ToString("dd MMMM yyyy") },
            { "Last updated", content.Sys.UpdatedAt?.ToString("dd MMMM yyyy") }
        };

        void AddProperty(string key, string prefix)
        {
            var propertyTags = tags.ContentCollection.Items.First().ContentfulMetaData.Tags.Where(x => x.Name.StartsWith($"{prefix}:"));
            if (propertyTags.Count() > 0)
            {
                var value = string.Join(", ", propertyTags.Select(x => x.Name.Split(":")[1].Trim(' ')));
                properties.Add(key, value);
            }
        }

        AddProperty("From", "Resource provider");
        AddProperty("Resource type", "Format");
        AddProperty("Topic", "Topic");
        AddProperty("Career stage", "Career stage");

        if (content.EstimatedReadingTime.HasValue && content.EstimatedReadingTime > 0)
        {
            properties.Add("Estimated reading time", $"{content.EstimatedReadingTime} mins");
        }

        return properties;
    }

    [Route("resources-learning/{*pagename:regex(^[[0-9a-z]](\\/?[[0-9a-z\\-]])*\\/?$)}")]
    public async Task<IActionResult> Index(string pageName = "home", bool preferenceSet = false, CancellationToken cancellationToken = default)
    {
        if (!_featuresConfig.IsEnabled(Features.ResourcesAndLearning))
        {
            return NotFound();
        }

        pageName = $"resources-learning/{pageName?.TrimEnd('/')}";
        (var content, var tags) = await _resourcesRepository.GetByIdAsync(pageName, cancellationToken: cancellationToken);
        if (content == null)
        {
            return NotFound();
        }

        var contextModel = new ContextModel(
            Id: content.Id,
            Title: content.Title,
            PageName: pageName,
            Category: content.Category,
            UseContainers: content.Navigation == null,
            PreferenceSet: preferenceSet,
            BackLink: content.BackLink);

        ViewData["ContextModel"] = contextModel;
        ViewData["StateModel"] = new StateModel();
        ViewData["Properties"] = GetProperties(tags, content);

        return View("Resource", content);
    }
}
