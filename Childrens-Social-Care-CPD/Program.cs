using Azure.Identity;
using Contentful.AspNetCore;

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
