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
    private ICpdContentfulClient _contentfulClient;
    private IApplicationConfiguration _applicationConfiguration;
    private IFeaturesConfiguration _featuresConfiguration;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<FeatureConfigurationPoller>>();
        _contentfulClient = Substitute.For<ICpdContentfulClient>();
        _applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        _featuresConfiguration = Substitute.For<IFeaturesConfiguration>();
    }

    [Test]
    public async Task Poll_Updates_Features()
    {
        // arrange
        var featureName = "foo";
        var applicationFeatures = new ApplicationFeatures
        {
            Features = new List<ApplicationFeature>
            {
                new ApplicationFeature
                {
                    Name = featureName,
                    IsEnabled = true
                }
            }
        };

        _contentfulClient
            .GetEntries(Arg.Any<QueryBuilder<ApplicationFeatures>>(), Arg.Any<CancellationToken>())
            .Returns(
                new ContentfulCollection<ApplicationFeatures>
                {
                    Items = new List<ApplicationFeatures> {
                        applicationFeatures
                    }
                }
            );
        
        _applicationConfiguration.FeaturePollingInterval.Returns(1);
        var cancellationTokenSource = new CancellationTokenSource();
        var poller = new FeatureConfigurationPoller(
            _logger,
            _contentfulClient,
            _applicationConfiguration,
            _featuresConfiguration
        );

        // act
        var task = poller.StartAsync(cancellationTokenSource.Token);
        await Task.Delay(25);
        cancellationTokenSource.Cancel();
        await task;

        // assert
        _featuresConfiguration.Received().AddOrUpdateFeature(featureName, true);
    }

    [Test]
    public async Task Poll_Ignores_Empty_Response()
    {
        // arrange
        _contentfulClient
            .GetEntries(Arg.Any<QueryBuilder<ApplicationFeatures>>(), Arg.Any<CancellationToken>())
            .Returns(
                new ContentfulCollection<ApplicationFeatures>
                {
                    Items = new List<ApplicationFeatures>()
                }
            );

        _applicationConfiguration.FeaturePollingInterval.Returns(1);
        var cancellationTokenSource = new CancellationTokenSource();
        var poller = new FeatureConfigurationPoller(
            _logger,
            _contentfulClient,
            _applicationConfiguration,
            _featuresConfiguration
        );

        // act
        var task = poller.StartAsync(cancellationTokenSource.Token);
        await Task.Delay(25);
        cancellationTokenSource.Cancel();
        await task;

        // assert
        _featuresConfiguration.DidNotReceive().AddOrUpdateFeature(Arg.Any<string>(), Arg.Any<bool>());
    }

    [Test]
    public async Task Poll_catches_exceptions()
    {
        // arrange
        var exception = new TestException();
        _contentfulClient
            .GetEntries(Arg.Any<QueryBuilder<ApplicationFeatures>>(), Arg.Any<CancellationToken>())
            .Throws(exception);

        _applicationConfiguration.FeaturePollingInterval.Returns(1);
        var cancellationTokenSource = new CancellationTokenSource();
        var poller = new FeatureConfigurationPoller(
            _logger,
            _contentfulClient,
            _applicationConfiguration,
            _featuresConfiguration
        );

        // act
        var task = poller.StartAsync(cancellationTokenSource.Token);
        await Task.Delay(25);
        cancellationTokenSource.Cancel();
        await task;

        // assert
        _logger.Received().LogError(exception, "Features Poller: exception querying for feature configuration. Does the FeatureConfiguration model exist in Contentful?");
    }
}
