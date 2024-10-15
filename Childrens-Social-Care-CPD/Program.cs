using Childrens_Social_Care_CPD;
using Childrens_Social_Care_CPD.Configuration.Features;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);

var sw = new Stopwatch();
sw.Start();

builder.AddDependencies();
Console.WriteLine($"After AddDependencies {sw.ElapsedMilliseconds}ms");

builder.AddFeatures(sw);
Console.WriteLine($"After AddFeatures {sw.ElapsedMilliseconds}ms");

builder.Services.AddDistributedMemoryCache();
Console.WriteLine($"After AddDistributedMemoryCache {sw.ElapsedMilliseconds}ms");

builder.Services.AddSession();
Console.WriteLine($"After AddSession {sw.ElapsedMilliseconds}ms");

var app = builder.Build();
Console.WriteLine($"After Application Build {sw.ElapsedMilliseconds}ms");

// Ensure the features are fetched before starting the app
var updater = app.Services.GetService<IFeaturesConfigUpdater>();
using (var cts = new CancellationTokenSource())
{
    await updater.UpdateFeaturesAsync(cts.Token);
}
Console.WriteLine($"After UpdateFeaturesAsync {sw.ElapsedMilliseconds}ms");

app.UseResponseCompression();
Console.WriteLine($"After UseResponseCompression {sw.ElapsedMilliseconds}ms");

app.UseExceptionHandler("/error/error");
Console.WriteLine($"After UseExceptionHandler {sw.ElapsedMilliseconds}ms");

app.UseStatusCodePagesWithReExecute("/error/{0}");
Console.WriteLine($"After UseStatusCodePagesWithReExecute{sw.ElapsedMilliseconds}ms");

app.UseStaticFiles();
Console.WriteLine($"After UseStaticFiles {sw.ElapsedMilliseconds}ms");

app.UseRouting();
Console.WriteLine($"After UseRouting {sw.ElapsedMilliseconds}ms");

app.UseAuthorization();
Console.WriteLine($"After UseAuthorization {sw.ElapsedMilliseconds}ms");

app.UseSession();
Console.WriteLine($"After UseSession {sw.ElapsedMilliseconds}ms");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Content}/{action=Index}");
Console.WriteLine($"After MapControllerRoute {sw.ElapsedMilliseconds}ms");

app.MapHealthChecks("application/status");
Console.WriteLine($"After MapHealthChecks {sw.ElapsedMilliseconds}ms");

app.Run();
Console.WriteLine($"After Application Run {sw.ElapsedMilliseconds}ms");

[ExcludeFromCodeCoverage]
public partial class Program
{
    protected Program() { }
}