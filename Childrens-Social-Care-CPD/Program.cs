using Childrens_Social_Care_CPD;
using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);
builder.AddDependencies();
builder.AddFeatures();

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
public partial class Program
{
    protected Program() { }
}