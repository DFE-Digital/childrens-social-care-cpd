using Childrens_Social_Care_CPD.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Threading;
using System.Threading.Tasks;
namespace Childrens_Social_Care_CPD_Tests.Configuration;

public class FeaturesConfigBackgroundServiceTests
{
    private ILogger<FeaturesConfigBackgroundService> _logger;
    private IApplicationConfiguration _applicationConfiguration;
    private IFeaturesConfigUpdater _featuresConfigUpdater;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<FeaturesConfigBackgroundService>>();

        _applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        _featuresConfigUpdater = Substitute.For<IFeaturesConfigUpdater>();
    }

    [TestCase(500)]
    [TestCase(1000)]
    public async Task Calls_Updater_At_Specified_Interval(int interval)
    {
        // arrange
        _applicationConfiguration.FeaturePollingInterval.Returns(interval);
        var featuresConfigBackgroundService = new FeaturesConfigBackgroundService(
            _logger,
            _applicationConfiguration,
            _featuresConfigUpdater
        );

        // act
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            var task = featuresConfigBackgroundService.StartAsync(cancellationTokenSource.Token);
            await Task.Delay((int)(interval * 1.1));
            cancellationTokenSource.Cancel();
            task.Wait();
        }

        // assert
        await _featuresConfigUpdater.Received(1).UpdateFeaturesAsync(Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Returns_If_Interval_Is_Zero()
    {
        // arrange
        _applicationConfiguration.FeaturePollingInterval.Returns(0);
        var featuresConfigBackgroundService = new FeaturesConfigBackgroundService(
            _logger,
            _applicationConfiguration,
            _featuresConfigUpdater
        );

        // act
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            var task = featuresConfigBackgroundService.StartAsync(cancellationTokenSource.Token);
            await Task.Delay(5000);
            cancellationTokenSource.Cancel();
            task.Wait();
        }

        // assert
        await _featuresConfigUpdater.DidNotReceive().UpdateFeaturesAsync(Arg.Any<CancellationToken>());
    }
}