using Childrens_Social_Care_CPD.Configuration;
using FluentAssertions;
using NUnit.Framework;

namespace Childrens_Social_Care_CPD_Tests.Configuration;

public class FeaturesConfigurationTest
{
    [Test]
    public void UpdateFeatureConfig_Adds_New_Feature()
    {
        // arrange
        var featureName = "foo";
        var featuresConfiguration = new FeaturesConfiguration();

        // act
        featuresConfiguration.AddOrUpdateFeature(featureName, true);

        // assert
        featuresConfiguration.IsEnabled(featureName).Should().BeTrue();
    }

    [Test]
    public void IsEnabled_Returns_False_For_Non_Existant_Features()
    {
        // arrange
        var featuresConfiguration = new FeaturesConfiguration();

        // assert
        featuresConfiguration.IsEnabled("foo").Should().BeFalse();
    }

    [Test]
    public void UpdateFeatureConfig_Updates_Features()
    {
        // arrange
        var featureName = "foo";
        var featuresConfiguration = new FeaturesConfiguration();

        // act
        featuresConfiguration.AddOrUpdateFeature(featureName, false);
        featuresConfiguration.AddOrUpdateFeature(featureName, true);

        // assert
        featuresConfiguration.IsEnabled("foo").Should().BeTrue();
    }
}
