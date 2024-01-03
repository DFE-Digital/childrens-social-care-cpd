﻿using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.DataAccess;
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

        var properties = new Dictionary<string, string>(tags.ContentCollection.Items.First().ContentfulMetaData.Tags.Where(x => x.Name.StartsWith("Resource:")).Select(x =>
        {
            var property = x.Name[9..];
            var tokens = property.Split('=');
            return tokens.Length > 1
                ? KeyValuePair.Create(tokens[0].Trim(' '), tokens[1].Trim(' '))
                : KeyValuePair.Create(property, string.Empty);
        }))
        {
            { "Published", content.Sys.CreatedAt?.ToString("dd MMMM yyyy") },
            { "Last updated", content.Sys.UpdatedAt?.ToString("dd MMMM yyyy") }
        };

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
        ViewData["Properties"] = properties;

        return View("Resource", content);
    }
}
