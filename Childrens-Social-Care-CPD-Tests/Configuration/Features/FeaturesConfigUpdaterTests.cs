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
using Childrens_Social_Care_CPD.Configuration.Features;

namespace Childrens_Social_Care_CPD_Tests.Configuration;

public class FeaturesConfigurationUpdaterTest
{
    private ILogger<FeaturesConfigUpdater> _logger;
    private ICpdContentfulClient _contentfulClient;
    private IFeaturesConfig _featuresConfig;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<FeaturesConfigUpdater>>();
        _contentfulClient = Substitute.For<ICpdContentfulClient>();
        _featuresConfig = Substitute.For<IFeaturesConfig>();
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

        var featuresConfigUpdater = new FeaturesConfigUpdater(_logger, _contentfulClient, _featuresConfig);

        // act
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            await featuresConfigUpdater.UpdateFeaturesAsync(cancellationTokenSource.Token);
        }

        // assert
        _featuresConfig.Received().AddOrUpdateFeature(featureName, true);
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

        var featuresConfigUpdater = new FeaturesConfigUpdater(_logger, _contentfulClient, _featuresConfig);

        // act
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            await featuresConfigUpdater.UpdateFeaturesAsync(cancellationTokenSource.Token);
        }

        // assert
        _featuresConfig.DidNotReceive().AddOrUpdateFeature(Arg.Any<string>(), Arg.Any<bool>());
    }

    [Test]
    public async Task Poll_catches_exceptions()
    {
        // arrange
        var exception = new TestException();
        _contentfulClient
            .GetEntries(Arg.Any<QueryBuilder<ApplicationFeatures>>(), Arg.Any<CancellationToken>())
            .Throws(exception);

        var featuresConfigUpdater = new FeaturesConfigUpdater(_logger, _contentfulClient, _featuresConfig);

        // act
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            await featuresConfigUpdater.UpdateFeaturesAsync(cancellationTokenSource.Token);
        }

        // assert
        _logger.Received().LogError(exception, "Exception querying for feature configuration. Does the ApplicationFeatures model exist in Contentful?");
    }
}
