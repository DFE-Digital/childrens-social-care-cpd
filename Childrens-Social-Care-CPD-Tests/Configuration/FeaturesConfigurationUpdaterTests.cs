using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful;
using Childrens_Social_Care_CPD.Contentful.Models;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using NSubstitute.ExceptionExtensions;

namespace Childrens_Social_Care_CPD_Tests.Configuration;

public class FeaturesConfigurationUpdaterTest
{
    private ILogger<FeatureConfigurationUpdater> _logger;
    private ICpdContentfulClient _contentfulClient;
    private IFeaturesConfiguration _featuresConfiguration;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<FeatureConfigurationUpdater>>();
        _contentfulClient = Substitute.For<ICpdContentfulClient>();
        _featuresConfiguration = Substitute.For<IFeaturesConfiguration>();
    }

    [Test]
    public async Task Updates_Features()
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

        var featureConfigurationUpdater = new FeatureConfigurationUpdater(_logger, _contentfulClient, _featuresConfiguration);

        // act
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            await featureConfigurationUpdater.UpdateFeaturesAsync(cancellationTokenSource.Token);
        }

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

        var featureConfigurationUpdater = new FeatureConfigurationUpdater(_logger, _contentfulClient, _featuresConfiguration);

        // act
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            await featureConfigurationUpdater.UpdateFeaturesAsync(cancellationTokenSource.Token);
        }

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

        var featureConfigurationUpdater = new FeatureConfigurationUpdater(_logger, _contentfulClient, _featuresConfiguration);

        // act
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            await featureConfigurationUpdater.UpdateFeaturesAsync(cancellationTokenSource.Token);
        }

        // assert
        _logger.Received().LogError(exception, "Exception querying for feature configuration. Does the FeatureConfiguration model exist in Contentful?");
    }
}
