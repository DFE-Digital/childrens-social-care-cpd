using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Childrens_Social_Care_CPD.Interfaces;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using System.Collections.Generic;

namespace Childrens_Social_Care_CPD_Tests;

internal class CpdTestServerApplication : WebApplicationFactory<Program>
{
    private IContentfulDataService _contentfulDataService;
    private ICpdContentfulClient _cpdContentfulClient;

    public CpdTestServerApplication()
    {
        _contentfulDataService = Substitute.For<IContentfulDataService>();
        _cpdContentfulClient = Substitute.For<ICpdContentfulClient>();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddTransient((_) => _contentfulDataService);
            services.AddTransient((_) => _cpdContentfulClient);
        });
        return base.CreateHost(builder);
    }

    public IContentfulDataService ContentfulDataService => _contentfulDataService;
    public ICpdContentfulClient CpdContentfulClient => _cpdContentfulClient;
}
