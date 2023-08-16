using Childrens_Social_Care_CPD;
using Childrens_Social_Care_CPD.Contentful;
using Contentful.AspNetCore;
using Contentful.Core.Configuration;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Logging.AzureAppServices;
using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogging(logging => logging.AddAzureWebAppDiagnostics())
.ConfigureServices(serviceCollection => serviceCollection
    .Configure<AzureFileLoggerOptions>(options =>
    {
        options.FileName = "azure-diagnostics-";
        options.FileSizeLimit = 50 * 1024;
        options.RetainedFileCountLimit = 5;
    }).Configure<AzureBlobLoggerOptions>(options =>
    {
        options.BlobName = "log.txt";
    })
);

// Add services to the container.
var applicationConfiguration = new ApplicationConfiguration();

builder.Services.AddResponseCompression();
builder.Services.AddControllersWithViews();
builder.Services.AddContentful(ContentfulConfiguration.GetContentfulConfiguration(builder.Configuration, applicationConfiguration));
builder.Services.AddTransient<IContentTypeResolver, EntityResolver>();
builder.Services.AddTransient<ICpdContentfulClient, CpdContentfulClient>();
builder.Services.AddSingleton<IApplicationConfiguration>(applicationConfiguration);
builder.Services.AddSingleton<ICookieHelper, CookieHelper>();

var options = new ApplicationInsightsServiceOptions
{
    ConnectionString = applicationConfiguration.AppInsightsConnectionString
};

builder.Services.AddApplicationInsightsTelemetry(options: options);
var app = builder.Build();

app.UseResponseCompression();
app.UseExceptionHandler("/error/error");
app.UseStatusCodePagesWithReExecute("/error/{0}");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Content}/{action=Index}");

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program { }