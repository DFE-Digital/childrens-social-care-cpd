using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Configuration.Features;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.DataAccess;
using Childrens_Social_Care_CPD.GraphQL.Queries;
using Childrens_Social_Care_CPD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Childrens_Social_Care_CPD.Controllers;

public class ResourcesController(IFeaturesConfig featuresConfig, IResourcesRepository resourcesRepository) : Controller
{
    private static Dictionary<string, string> GetProperties(GetContentTags.ResponseType tags, Content content)
    {
        var properties = new Dictionary<string, string>()
        {
            { "Published", content.Sys.CreatedAt?.ToString("dd MMMM yyyy") },
            { "Last updated", content.Sys.UpdatedAt?.ToString("dd MMMM yyyy") }
        };

        void AddProperty(string key, string prefix)
        {
            var propertyTags = tags.ContentCollection.Items.First().ContentfulMetaData.Tags.Where(x => x.Name.StartsWith($"{prefix}:"));
            if (propertyTags.Any())
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

    [Route("resources-learning/{*pagename:regex(^[[0-9a-z]]+[[0-9a-z\\/\\-]]*$)}")]
    public async Task<IActionResult> Index(string pageName = "home", bool preferenceSet = false, bool fs = false, CancellationToken cancellationToken = default)
    {
        if (!featuresConfig.IsEnabled(Features.ResourcesAndLearning))
        {
            return NotFound();
        }

        pageName = $"resources-learning/{pageName?.TrimEnd('/')}";
        (var content, var tags) = await resourcesRepository.GetByIdAsync(pageName, cancellationToken: cancellationToken);
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
            BackLink: content.BackLink,
            FeedbackSubmitted: fs);

        ViewData["ContextModel"] = contextModel;
        ViewData["StateModel"] = new StateModel();
        ViewData["Properties"] = GetProperties(tags, content);

        return View("Resource", content);
    }
}
