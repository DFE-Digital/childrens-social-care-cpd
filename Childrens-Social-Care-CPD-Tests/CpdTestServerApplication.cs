using Childrens_Social_Care_CPD.Contentful;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

namespace Childrens_Social_Care_CPD_Tests;

internal class CpdTestServerApplication : WebApplicationFactory<Program>
{
    private readonly ICpdContentfulClient _cpdContentfulClient;
    private ILoggerFactory _loggerFactory;

    public CpdTestServerApplication()
    {
        ClientOptions.AllowAutoRedirect = false;
        ClientOptions.HandleCookies = true;
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

            services.AddControllers()
                    .AddApplicationPart(typeof(AntiforgeryTokenController).Assembly)
                    .AddControllersAsServices();
        });
        return base.CreateHost(builder);
    }

    public ICpdContentfulClient CpdContentfulClient => _cpdContentfulClient;
    public ILoggerFactory LoggerFactory => _loggerFactory;

    public async Task<AntiforgeryTokens> GetAntiforgeryTokensAsync(CancellationToken cancellationToken = default)
    {
        using var httpClient = CreateClient();
        using var response = await httpClient.GetAsync(AntiforgeryTokenController.GetTokensUri, cancellationToken).ConfigureAwait(false);

        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        return JsonSerializer.Deserialize<AntiforgeryTokens>(json);
    }
}
