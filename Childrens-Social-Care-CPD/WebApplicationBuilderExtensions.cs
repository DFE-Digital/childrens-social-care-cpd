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
            var client = new GraphQLHttpClient(config.ContentfulGraphqlConnectionString.Value, new SystemTextJsonSerializer());
            var key = ContentfulConfiguration.IsPreviewEnabled(config) ? config.ContentfulPreviewId.Value : config.ContentfulDeliveryApiKey.Value;
            
            client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
            return client;
        });

        builder.Services.AddScoped<IContentLinkContext, ContentLinkContext>();

        builder.Services.AddContentRenderers();
        builder.Services.AddSearch();
    }

    private static void AddContentRenderers(this IServiceCollection services)
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

    private static void AddSearch(this IServiceCollection services)
    {
        services.AddScoped<ISearchResultsVMFactory, SearchResultsVMFactory>();

        services.AddScoped(services => {
            var config = services.GetRequiredService<IApplicationConfiguration>();

            var searchEndpointUri = new Uri(config.SearchEndpoint.Value);
            return new SearchClient(searchEndpointUri,
                config.SearchIndexName.Value,
                new AzureKeyCredential(config.SearchApiKey.Value));
        });

        services.AddScoped<ISearchService, SearchService>();
    }

    public static void AddFeatures(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Configuration.AddUserSecrets<Program>();

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

        if (applicationConfiguration.AzureDataProtectionContainerName.IsSet)
        {
            var url = string.Format(applicationConfiguration.AzureStorageAccountUriFormatString.Value,
                applicationConfiguration.AzureStorageAccount.Value, 
                applicationConfiguration.AzureDataProtectionContainerName.Value);

            var managedIdentityCredential = new ManagedIdentityCredential(clientId: applicationConfiguration.AzureManagedIdentityId.Value);

            //var blobContainerUri = new Uri(url);
            //var blobContainerClient = new BlobContainerClient(blobContainerUri, managedIdentityCredential);
            //blobContainerClient.CreateIfNotExists();

            var blobUri = new Uri($"{url}/data-protection");
            builder.Services
                .AddDataProtection()
                .SetApplicationName($"Childrens-Social-Care-CPD-{applicationConfiguration.AzureEnvironment.Value}")
                .PersistKeysToAzureBlobStorage(blobUri, managedIdentityCredential);
                //.ProtectKeysWithAzureKeyVault("<keyid>", defaultAzureCredential); // TODO: add key vault encryption once blob storage has been tested
        }
    }
}