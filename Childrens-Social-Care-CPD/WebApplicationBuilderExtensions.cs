using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Renderers;
using Contentful.AspNetCore;
using Contentful.Core.Configuration;
using Microsoft.Extensions.Logging.AzureAppServices;
using System.Diagnostics.CodeAnalysis;

namespace Childrens_Social_Care_CPD;

[ExcludeFromCodeCoverage]
public static class WebApplicationBuilderExtensions
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        
        builder.Services.AddSingleton<IApplicationConfiguration, ApplicationConfiguration>();
        builder.Services.AddTransient<IContentTypeResolver, EntityResolver>();
        builder.Services.AddTransient<ICpdContentfulClient, CpdContentfulClient>();
        builder.Services.AddSingleton<ICookieHelper, CookieHelper>();
        builder.Services.AddTransient<IFeaturesConfig, FeaturesConfig>();
        builder.Services.AddTransient<IFeaturesConfigUpdater, FeaturesConfigUpdater>();

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
        builder.Logging.AddApplicationInsights(
            configureTelemetryConfiguration: (config) => {
                if (!string.IsNullOrEmpty(applicationConfiguration.AppInsightsConnectionString))
                {
                    config.ConnectionString = applicationConfiguration.AppInsightsConnectionString;
                }
            },
            configureApplicationInsightsLoggerOptions: (options) => { }
        );
    }
}
