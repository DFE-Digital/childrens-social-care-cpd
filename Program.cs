using Azure.Identity;
using Contentful.AspNetCore;
using Contentful.AspNetCore.MiddleWare;
using Contentful.Core.Models;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
ConfigurationManager configuration = builder.Configuration;

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

var app = builder.Build();

if (enableContentfulIntegration)
{
    app.UseContentfulWebhooks(consumers =>
    {
        consumers.AddConsumer<dynamic>("PrototypeWebHook", "Entry", "create", (s) =>
        {
        //Code to notify someone a new entry has been created

        //Return an object that will be serialized into Json and viewable in the Contentful web app
        return new { Result = "Ok" };
        }
        );
    });
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
