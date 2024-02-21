using Childrens_Social_Care_CPD;
using Childrens_Social_Care_CPD.Configuration;
using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);
builder.AddDependencies();
await builder.AddFeatures();

var app = builder.Build();

// Ensure the features are fetched before starting the app
var updater = app.Services.GetService<IFeaturesConfigUpdater>();
using (var cts = new CancellationTokenSource())
{
    await updater.UpdateFeaturesAsync(cts.Token);
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
app.MapHealthChecks("application/status");


app.Run();

[ExcludeFromCodeCoverage]
public partial class Program
{
    protected Program() { }
}