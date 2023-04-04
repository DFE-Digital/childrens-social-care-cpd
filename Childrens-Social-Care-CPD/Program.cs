using Childrens_Social_Care_CPD;
using Childrens_Social_Care_CPD.ActionFilters;
using Childrens_Social_Care_CPD.Constants;
using Childrens_Social_Care_CPD.Interfaces;
using Childrens_Social_Care_CPD.Services;
using Contentful.AspNetCore;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddTransient<CPDActionFilter>();
builder.Services.AddContentful(ContentfulConfiguration.GetContentfulConfiguration(configuration));
builder.Services.AddTransient<IContentfulDataService, ContentfulDataService>();

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
}
app.UseExceptionHandler("/Error/Error");
app.UseStatusCodePagesWithRedirects("/Error/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CPD}/{action=LandingPage}");

app.Run();
