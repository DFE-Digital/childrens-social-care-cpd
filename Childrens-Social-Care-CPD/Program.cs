using Azure.Identity;
using Childrens_Social_Care_CPD;
using Childrens_Social_Care_CPD.Constants;
using Childrens_Social_Care_CPD.Enums;
using Contentful.AspNetCore;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
ConfigurationManager configuration = builder.Configuration;
builder.WebHost.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");
builder.WebHost.CaptureStartupErrors(true);

bool enableContentfulIntegration = configuration.GetValue<bool>("EnableContentfulIntegration");
var contentfulEnvironment = Environment.GetEnvironmentVariable(SiteConstants.ENVIRONMENT) ?? String.Empty;

if (enableContentfulIntegration)
{
    var appEnvironment = Environment.GetEnvironmentVariable(SiteConstants.AZUREENVIRONMENT) ?? String.Empty;
    var deliveryApiKey = Environment.GetEnvironmentVariable(SiteConstants.DELIVERYAPIKEY) ?? String.Empty;
    var spaceId = Environment.GetEnvironmentVariable(SiteConstants.CONTENTFULSPACEID) ?? String.Empty;

    configuration["ContentfulOptions:Environment"] = contentfulEnvironment;
    configuration["ContentfulOptions:SpaceId"] = spaceId;
    configuration["ContentfulOptions:DeliveryApiKey"] = deliveryApiKey;

    if ((contentfulEnvironment.ToLower() != appEnvironment.ToLower()) && !String.IsNullOrEmpty(appEnvironment))
    {
        var previewApiKey = Environment.GetEnvironmentVariable(SiteConstants.PREVIEWAPIKEY) ?? String.Empty;
        configuration["ContentfulOptions:host"] = SiteConstants.CONTENTFULPREVIEWHOST;
        configuration["ContentfulOptions:UsePreviewApi"] = "true";
        configuration["ContentfulOptions:PreviewApiKey"] = previewApiKey;
    }

  
    builder.Services.AddContentful(configuration);
}

var options = new ApplicationInsightsServiceOptions {
    ConnectionString = Environment.GetEnvironmentVariable(SiteConstants.CPD_INSTRUMENTATION_CONNECTIONSTRING)??String.Empty
};

builder.Services.AddApplicationInsightsTelemetry(options: options);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseExceptionHandler("/Error/Error");
app.UseStatusCodePagesWithRedirects("/Error/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
if (contentfulEnvironment != AppEnvironment.dev.ToString())
{
    app.UseMiddleware<CheckRequestHeaderMiddleware>();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CPD}/{action=LandingPage}");

app.Run();
