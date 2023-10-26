using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Renderers;
using Childrens_Social_Care_CPD.DataAccess;
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
        builder.Services.AddTransient<IResourcesRepository,  ResourcesRepository>();

        builder.Services.AddScoped<IGraphQLWebSocketClient>(services => {
            var config = services.GetService<IApplicationConfiguration>();
            var client = new GraphQLHttpClient(config.ContentfulGraphqlConnectionString.Value, new SystemTextJsonSerializer());
            var key = ContentfulConfiguration.IsPreviewEnabled(config) ? config.ContentfulPreviewId.Value : config.ContentfulDeliveryApiKey.Value;
            
            client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
            return client;
        });

        // Register all the IRender<T> implementations in the assembly
        System.Reflection.Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(item => item.GetInterfaces().Where(i => i.IsGenericType).Any(i => i.GetGenericTypeDefinition() == typeof(IRenderer<>)) && !item.IsAbstract && !item.IsInterface)
            .ToList()
            .ForEach(assignedTypes =>
            {
                var serviceType = assignedTypes.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IRenderer<>));
                builder.Services.AddScoped(serviceType, assignedTypes);
            });
    }

    public static void AddFeatures(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var applicationConfiguration = new ApplicationConfiguration();

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
