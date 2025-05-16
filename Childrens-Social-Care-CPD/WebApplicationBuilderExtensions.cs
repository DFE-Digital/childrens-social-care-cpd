using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Configuration.Features;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Contexts;
using Childrens_Social_Care_CPD.Contentful.Renderers;
using Contentful.AspNetCore;
using Contentful.Core.Configuration;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Logging.AzureAppServices;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

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
        builder.Services.AddScoped<IContentLinkContext, ContentLinkContext>();

        AddContentRenderers(builder.Services);
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

    public static void AddFeatures(this WebApplicationBuilder builder, Stopwatch sw)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Configuration.AddUserSecrets<Program>();
        Console.WriteLine($"After AddUserSecrets: {sw.ElapsedMilliseconds}ms");

        builder.Services.AddResponseCompression();
        Console.WriteLine($"After AddResponseCompression: {sw.ElapsedMilliseconds}ms");

        builder.Services.AddControllersWithViews();
        Console.WriteLine($"After AddControllersWithViews: {sw.ElapsedMilliseconds}ms");

        var applicationConfiguration = new ApplicationConfiguration(builder.Configuration);
        Console.WriteLine($"After ApplicationConfiguration creation: {sw.ElapsedMilliseconds}ms");

        AddLogging(builder, applicationConfiguration);
        Console.WriteLine($"After AddLogging: {sw.ElapsedMilliseconds}ms");

        AddContentful(builder, applicationConfiguration);
        Console.WriteLine($"After AddContentful: {sw.ElapsedMilliseconds}ms");

        AddHostedServices(builder.Services);
        Console.WriteLine($"After AddHostedServices: {sw.ElapsedMilliseconds}ms");

        AddHealthChecks(builder.Services);
        Console.WriteLine($"After AddHealthChecks: {sw.ElapsedMilliseconds}ms");
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
#pragma warning disable CA1861 // Avoid constant arrays as arguments
        services.AddHealthChecks().AddCheck<ConfigurationHealthCheck>("Configuration Health Check", tags: new[] { "configuration" });
#pragma warning restore CA1861 // Avoid constant arrays as arguments
    }
}