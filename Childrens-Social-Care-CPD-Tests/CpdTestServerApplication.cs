using Childrens_Social_Care_CPD.Contentful;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Threading;

namespace Childrens_Social_Care_CPD_Tests;

internal class CpdTestServerApplication : WebApplicationFactory<Program>
{
    private ICpdContentfulClient _cpdContentfulClient;
    private ILoggerFactory _loggerFactory;

    public CpdTestServerApplication()
    {
        _cpdContentfulClient = Substitute.For<ICpdContentfulClient>();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        _loggerFactory = Substitute.For<ILoggerFactory>();
        builder.ConfigureServices(services =>
        {
            services.AddTransient((_) => _cpdContentfulClient);
            services.AddSingleton(_loggerFactory);
            services.AddScoped(typeof(CancellationToken), s => new CancellationTokenSource().Token);
        });
        return base.CreateHost(builder);
    }

    public ICpdContentfulClient CpdContentfulClient => _cpdContentfulClient;
    public ILoggerFactory LoggerFactory => _loggerFactory;
}
