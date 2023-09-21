using Childrens_Social_Care_CPD.Configuration;
using FluentAssertions;
using NUnit.Framework;

namespace Childrens_Social_Care_CPD_Tests.Configuration;

public class FeaturesConfigTests
{
    [Test]
    public void UpdateFeaturesConfig_Adds_New_Feature()
    {
        // arrange
        var featureName = "foo";
        var featuresConfig = new FeaturesConfig();

        // act
        featuresConfig.AddOrUpdateFeature(featureName, true);

        // assert
        featuresConfig.IsEnabled(featureName).Should().BeTrue();
    }

    [Test]
    public void IsEnabled_Returns_False_For_Non_Existant_Features()
    {
        // arrange
        var featuresConfig = new FeaturesConfig();

        // assert
        featuresConfig.IsEnabled("foo").Should().BeFalse();
    }

    [Test]
    public void UpdateFeaturesConfig_Updates_Features()
    {
        // arrange
        var featureName = "foo";
        var featuresConfig = new FeaturesConfig();

        // act
        featuresConfig.AddOrUpdateFeature(featureName, false);
        featuresConfig.AddOrUpdateFeature(featureName, true);

        // assert
        featuresConfig.IsEnabled("foo").Should().BeTrue();
    }
}
