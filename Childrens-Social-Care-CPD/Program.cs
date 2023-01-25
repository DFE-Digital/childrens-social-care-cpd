using Azure.Identity;
using Childrens_Social_Care_CPD.Constants;
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

if (enableContentfulIntegration)
{
    var contentfulEnvironment = Environment.GetEnvironmentVariable("CPD_CONTENTFUL_ENVIRONMENT") ?? String.Empty;
    var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("CPD_KEYVAULTENDPOINT")?? String.Empty);
    var clientId = Environment.GetEnvironmentVariable("CPD_CLIENTID") ?? String.Empty;
    var clientSecret = Environment.GetEnvironmentVariable("CPD_CLIENTSECRET") ?? String.Empty;
    var tenantId = Environment.GetEnvironmentVariable("CPD_TENANTID") ?? String.Empty;

    configuration["ContentfulOptions:Environment"] = contentfulEnvironment;

    builder.Services.AddContentful(configuration);

    var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

    builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, clientSecretCredential);
}

var options = new ApplicationInsightsServiceOptions { ConnectionString = "InstrumentationKey=13cbca44-e50d-4444-ab56-6c151cad789d;IngestionEndpoint=https://westeurope-5.in.applicationinsights.azure.com/;LiveEndpoint=https://westeurope.livediagnostics.monitor.azure.com/" };
builder.Services.AddApplicationInsightsTelemetry(options: options);

var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
telemetryConfiguration.ConnectionString = Environment.GetEnvironmentVariable(SiteConstants.CPD_INSTRUMENTATION_KEY);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseExceptionHandler("/CPD/Error");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CPD}/{action=LandingPage}");

app.Run();
