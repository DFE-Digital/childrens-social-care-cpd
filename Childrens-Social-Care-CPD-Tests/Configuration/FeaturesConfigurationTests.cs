using Childrens_Social_Care_CPD.Configuration;
using Childrens_Social_Care_CPD.Contentful.Models;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Childrens_Social_Care_CPD_Tests.Configuration;

public class FeaturesConfigurationTest
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void UpdateFeatureConfig_Throws_If_No_ApplicationFeatures()
    {
        // arrange
        Action action = () => FeaturesConfiguration.UpdateFeatureConfig(null);

        // act/assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void UpdateFeatureConfig_Does_Not_Throw_With_No_Features()
    {
        // arrange
        Action action = () => FeaturesConfiguration.UpdateFeatureConfig(new ApplicationFeatures());

        // act/assert
        action.Should().NotThrow<Exception>();
    }

    [Test]
    public void UpdateFeatureConfig_Adds_New_Feature()
    {
        // arrange
        var applicationFeatures = new ApplicationFeatures
        {
            Features = new List<ApplicationFeature>
            {
                new ApplicationFeature
                {
                    Name = "foo",
                    IsEnabled = true
                }
            }
        };
        var features = new FeaturesConfiguration();

        // act
        FeaturesConfiguration.UpdateFeatureConfig(applicationFeatures);

        // assert
        features.IsEnabled("foo").Should().BeTrue();
    }

    [Test]
    public void IsEnabled_Returns_False_For_Non_Existant_Features()
    {
        // arrange
        var features = new FeaturesConfiguration();

        // assert
        features.IsEnabled("foo").Should().BeFalse();
    }

    [Test]
    public void UpdateFeatureConfig_Updates_Features()
    {
        // arrange
        var feature = new ApplicationFeature
        {
            Name = "foo",
            IsEnabled = false
        };

        var applicationFeatures = new ApplicationFeatures
        {
            Features = new List<ApplicationFeature>
            {
                feature
            }
        };
        var features = new FeaturesConfiguration();

        // act
        FeaturesConfiguration.UpdateFeatureConfig(applicationFeatures);
        feature.IsEnabled = true;
        FeaturesConfiguration.UpdateFeatureConfig(applicationFeatures);

        // assert
        features.IsEnabled("foo").Should().BeTrue();
    }
}
