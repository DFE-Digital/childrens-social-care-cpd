using Azure.Search.Documents;
using Azure;
using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Contexts;
using Childrens_Social_Care_CPD.Contentful.Renderers;
using Childrens_Social_Care_CPD.DataAccess;
using Childrens_Social_Care_CPD.Search;
using Childrens_Social_Care_CPD.Services;
using Contentful.AspNetCore;
using Contentful.Core.Configuration;
using GraphQL.Client.Abstractions.Websocket;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Logging.AzureAppServices;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.DataProtection;
using Azure.Storage.Blobs;
using Azure.Identity;
using Childrens_Social_Care_CPD.Configuration.Features;

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
        
        builder.Services.AddScoped<IGraphQLWebSocketClient>(services => {
            var config = services.GetRequiredService<IApplicationConfiguration>();
            var client = new GraphQLHttpClient(config.ContentfulGraphqlConnectionString, new SystemTextJsonSerializer());
            var key = ContentfulConfiguration.IsPreviewEnabled(config) ? config.ContentfulPreviewId : config.ContentfulDeliveryApiKey;
            
            client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
            return client;
        });

        builder.Services.AddScoped<IContentLinkContext, ContentLinkContext>();

        AddContentRenderers(builder.Services);
        AddSearch(builder.Services);
    }

    private static void AddContentRenderers(IServiceCollection services)
    {
        // Register all the IRenderer<T> & IRendererWithOptions<T> implementations in the assembly
        var assemblyTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();

        assemblyTypes
            .Where(item => item.GetInterfaces().Where(i => i.IsGenericType).Any(i => i.GetGenericTypeDefinition() == typeof(IRenderer<>)) && !item.IsAbstract && !item.IsInterface)
            .ToList()
            .ForEach(assignedTypes =>
            {
                var serviceType = assignedTypes.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IRenderer<>));
                services.AddScoped(serviceType, assignedTypes);
            });

        assemblyTypes
            .Where(item => item.GetInterfaces().Where(i => i.IsGenericType).Any(i => i.GetGenericTypeDefinition() == typeof(IRendererWithOptions<>)) && !item.IsAbstract && !item.IsInterface)
            .ToList()
            .ForEach(assignedTypes =>
            {
                var serviceType = assignedTypes.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IRendererWithOptions<>));
                services.AddScoped(serviceType, assignedTypes);
            });
    }

    private static void AddSearch(IServiceCollection services)
    {
        services.AddScoped<ISearchResultsVMFactory, SearchResultsVMFactory>();

        services.AddScoped(services => {
            var config = services.GetRequiredService<IApplicationConfiguration>();

            var searchEndpointUri = new Uri(config.SearchEndpoint);
            return new SearchClient(searchEndpointUri,
                config.SearchIndexName,
                new AzureKeyCredential(config.SearchApiKey));
        });

        services.AddScoped<ISearchService, SearchService>();
    }

    public static async Task AddFeatures(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Configuration.AddUserSecrets<Program>();
        builder.Services.AddResponseCompression();
        builder.Services.AddControllersWithViews();

        var applicationConfiguration = new ApplicationConfiguration(builder.Configuration);

        AddLogging(builder, applicationConfiguration);
        AddContentful(builder, applicationConfiguration);
        AddHostedServices(builder.Services);
        AddHealthChecks(builder.Services);
        await AddDataProtection(builder.Services, applicationConfiguration);
    }

    private static void AddLogging(WebApplicationBuilder builder, ApplicationConfiguration applicationConfiguration)
    {
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

        var options = new ApplicationInsightsServiceOptions
        {
            ApplicationVersion = applicationConfiguration.AppVersion,
            ConnectionString = applicationConfiguration.AppInsightsConnectionString,
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
    }

    private static void AddContentful(WebApplicationBuilder builder, ApplicationConfiguration applicationConfiguration)
    {
        var contentfulConfiguration = ContentfulConfiguration.GetContentfulConfiguration(builder.Configuration, applicationConfiguration);
        builder.Services.AddContentful(contentfulConfiguration);
    }

    private static void AddHostedServices(IServiceCollection services)
    {
        services.AddHostedService<FeaturesConfigBackgroundService>();
    }

    private static void AddHealthChecks(IServiceCollection services)
    {
        services.AddHealthChecks().AddCheck<ConfigurationHealthCheck>("Configuration Health Check", tags: new[] { "configuration" });
    }

    private static async Task AddDataProtection(IServiceCollection services, ApplicationConfiguration applicationConfiguration)
    {
        if (!string.IsNullOrEmpty(applicationConfiguration.AzureDataProtectionContainerName))
        {
            var url = string.Format(applicationConfiguration.AzureStorageAccountUriFormatString,
                applicationConfiguration.AzureStorageAccount,
                applicationConfiguration.AzureDataProtectionContainerName);

            var managedIdentityCredential = new ManagedIdentityCredential(clientId: applicationConfiguration.AzureManagedIdentityId);

            var blobContainerUri = new Uri(url);
            var blobContainerClient = new BlobContainerClient(blobContainerUri, managedIdentityCredential);
            await blobContainerClient.CreateIfNotExistsAsync();

            var blobUri = new Uri($"{url}/data-protection");
            services
                .AddDataProtection()
                .SetApplicationName($"Childrens-Social-Care-CPD-{applicationConfiguration.AzureEnvironment}")
                .PersistKeysToAzureBlobStorage(blobUri, managedIdentityCredential);
                //.ProtectKeysWithAzureKeyVault("<keyid>", managedIdentityCredential); // TODO: add key vault encryption once blob storage has been tested
        }
    }
}