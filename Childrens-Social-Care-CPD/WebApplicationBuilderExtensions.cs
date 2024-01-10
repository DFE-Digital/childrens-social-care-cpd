using Azure.Search.Documents;
using Azure;
using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Contexts;
using Childrens_Social_Care_CPD.Contentful.Renderers;
using Childrens_Social_Care_CPD.Core.Resources;
using Childrens_Social_Care_CPD.DataAccess;
using Childrens_Social_Care_CPD.Search;
using Childrens_Social_Care_CPD.Services;
using Contentful.AspNetCore;
using Contentful.Core.Configuration;
using GraphQL.Client.Abstractions.Websocket;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Logging.AzureAppServices;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

namespace Childrens_Social_Care_CPD;

[ExcludeFromCodeCoverage]
public static class WebApplicationBuilderExtensions
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        
        builder.Services.AddSingleton<IApplicationConfiguration, ApplicationConfiguration>();
        builder.Services.AddSingleton<IContentTypeResolver, EntityResolver>();
        builder.Services.AddTransient<ICpdContentfulClient, CpdContentfulClient>();
        builder.Services.AddSingleton<ICookieHelper, CookieHelper>();
        builder.Services.AddTransient<IFeaturesConfig, FeaturesConfig>();
        builder.Services.AddTransient<IFeaturesConfigUpdater, FeaturesConfigUpdater>();
        builder.Services.AddTransient<IResourcesRepository, ResourcesRepository>();
        
        // Resources search feature
        builder.Services.AddScoped<ResourcesDynamicTagsSearchStategy>();
        builder.Services.AddScoped<ResourcesFixedTagsSearchStrategy>();
        builder.Services.AddScoped<IResourcesSearchStrategy>(services =>
        {
            var featuresConfig = services.GetRequiredService<IFeaturesConfig>();
            return featuresConfig.IsEnabled(Features.ResourcesUseDynamicTags)
                ? services.GetService<ResourcesDynamicTagsSearchStategy>()
                : services.GetService<ResourcesFixedTagsSearchStrategy>();
        });

        builder.Services.AddScoped<IGraphQLWebSocketClient>(services => {
            var config = services.GetRequiredService<IApplicationConfiguration>();
            var client = new GraphQLHttpClient(config.ContentfulGraphqlConnectionString.Value, new SystemTextJsonSerializer());
            var key = ContentfulConfiguration.IsPreviewEnabled(config) ? config.ContentfulPreviewId.Value : config.ContentfulDeliveryApiKey.Value;
            
            client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
            return client;
        });

        builder.Services.AddScoped<IContentLinkContext, ContentLinkContext>();

        // Register all the IRender<T> & IRenderWithOptions<T> implementations in the assembly
        var assemblyTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();

        assemblyTypes
            .Where(item => item.GetInterfaces().Where(i => i.IsGenericType).Any(i => i.GetGenericTypeDefinition() == typeof(IRenderer<>)) && !item.IsAbstract && !item.IsInterface)
            .ToList()
            .ForEach(assignedTypes =>
            {
                var serviceType = assignedTypes.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IRenderer<>));
                builder.Services.AddScoped(serviceType, assignedTypes);
            });

        assemblyTypes
            .Where(item => item.GetInterfaces().Where(i => i.IsGenericType).Any(i => i.GetGenericTypeDefinition() == typeof(IRendererWithOptions<>)) && !item.IsAbstract && !item.IsInterface)
            .ToList()
            .ForEach(assignedTypes =>
            {
                var serviceType = assignedTypes.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IRendererWithOptions<>));
                builder.Services.AddScoped(serviceType, assignedTypes);
            });


        // Search client
        builder.Services.AddSearch();
    }

    private static void AddSearch(this IServiceCollection services)
    {
        services.AddScoped(services => {
            var config = services.GetRequiredService<IApplicationConfiguration>();

            var searchEndpointUri = new Uri(config.SearchEndpoint.Value);
            return new SearchClient(searchEndpointUri,
                config.SearchIndexName.Value,
                new AzureKeyCredential(config.SearchApiKey.Value));
        });

        services.AddTransient<ISearchService, SearchService>();
    }

    public static void AddFeatures(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var applicationConfiguration = new ApplicationConfiguration(builder.Configuration);

        builder.Logging.AddAzureWebAppDiagnostics();
        builder.Services.Configure<AzureFileLoggerOptions>(options =>
        {
            options.FileName = "azure-diagnostics-";
            options.FileSizeLimit = 50 * 1024;
            options.RetainedFileCountLimit = 5;
        }).Configure<AzureBlobLoggerOptions>(options =>
        {
            options.BlobName = "log.txt";
        });

        builder.Services.AddResponseCompression();
        builder.Services.AddControllersWithViews();
        builder.Services.AddContentful(ContentfulConfiguration.GetContentfulConfiguration(builder.Configuration, applicationConfiguration));
        builder.Services.AddHostedService<FeaturesConfigBackgroundService>();

        var options = new ApplicationInsightsServiceOptions
        {
            ApplicationVersion = applicationConfiguration.AppVersion.Value,
            ConnectionString = applicationConfiguration.AppInsightsConnectionString.Value,
        };

        builder.Services.AddApplicationInsightsTelemetry(options: options);
        builder.Services.PostConfigure<LoggerFilterOptions>(options =>
        {
            var originalRule = options.Rules.FirstOrDefault(x => x.ProviderName == typeof(ApplicationInsightsLoggerProvider).FullName);
            if (originalRule != null)
            {
                options.Rules.Remove(originalRule);
                options.Rules.Insert(0, new LoggerFilterRule(typeof(ApplicationInsightsLoggerProvider).FullName, null, LogLevel.Information, null));
            }
        });
        
        builder.Services.AddHealthChecks().AddCheck<ConfigurationHealthCheck>("Configuration Health Check", tags: new[] {"configuration"});
    }
}
