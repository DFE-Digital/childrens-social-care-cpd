using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace Childrens_Social_Care_CPD_Tests.Configuration;

public class FeatureConfigurationPollerTests
{
    private ILogger<FeatureConfigurationPoller> _logger;
    private IApplicationConfiguration _applicationConfiguration;
    private IFeatureConfigurationUpdater _featureConfigurationUpdater;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<FeatureConfigurationPoller>>();

        _applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        _featureConfigurationUpdater = Substitute.For<IFeatureConfigurationUpdater>();
    }

    [TestCase(500)]
    [TestCase(1000)]
    public async Task Poller_Calls_Updater_At_Specified_Interval(int interval)
    {
        // arrange
        _applicationConfiguration.FeaturePollingInterval.Returns(interval);
        var featureConfigurationPoller = new FeatureConfigurationPoller(
            _logger,
            _applicationConfiguration,
            _featureConfigurationUpdater
        );

        // act
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            var task = featureConfigurationPoller.StartAsync(cancellationTokenSource.Token);
            await Task.Delay((int)(interval * 1.1));
            cancellationTokenSource.Cancel();
            task.Wait();
        }

        // assert
        await _featureConfigurationUpdater.Received(1).UpdateFeaturesAsync(Arg.Any<CancellationToken>());
    }
}