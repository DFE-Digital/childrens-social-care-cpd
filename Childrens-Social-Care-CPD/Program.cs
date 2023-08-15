using Childrens_Social_Care_CPD;
using Childrens_Social_Care_CPD.Contentful;
using Contentful.AspNetCore;
using Contentful.Core.Configuration;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Logging.AzureAppServices;

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
builder.Services.AddResponseCompression();
builder.Services.AddControllersWithViews();
builder.Services.AddContentful(ContentfulConfiguration.GetContentfulConfiguration(builder.Configuration));
builder.Services.AddTransient<IContentTypeResolver, EntityResolver>();
builder.Services.AddTransient<ICpdContentfulClient, CpdContentfulClient>();

var options = new ApplicationInsightsServiceOptions
{
    ConnectionString = Environment.GetEnvironmentVariable(SiteConstants.CPD_INSTRUMENTATION_CONNECTIONSTRING) ?? String.Empty
};

builder.Services.AddApplicationInsightsTelemetry(options: options);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseDeveloperExceptionPage();
}

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