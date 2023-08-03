using Childrens_Social_Care_CPD.Contentful;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
namespace Childrens_Social_Care_CPD_Tests;

internal class CpdTestServerApplication : WebApplicationFactory<Program>
{
    private ICpdContentfulClient _cpdContentfulClient;

    public CpdTestServerApplication()
    {
        _cpdContentfulClient = Substitute.For<ICpdContentfulClient>();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddTransient((_) => _cpdContentfulClient);
        });
        return base.CreateHost(builder);
    }

    public ICpdContentfulClient CpdContentfulClient => _cpdContentfulClient;
}
