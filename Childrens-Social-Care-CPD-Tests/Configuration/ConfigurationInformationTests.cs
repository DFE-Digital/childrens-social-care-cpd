using NUnit.Framework;
using Childrens_Social_Care_CPD.Configuration;
using FluentAssertions;
using System.Linq;

namespace Childrens_Social_Care_CPD_Tests.Configuration;

public partial class ConfigurationInformationTests
{
    [Test]
    public void Required_Values_Are_Detected()
    {
        // arrange
        var config = new TestConfigurationMock();
        config._appVersion = "foo";
        
        // act
        var sut = new ConfigurationInformation(config);
        var actual = sut.ConfigurationInfo.Single(x => x.Name == "AppVersion");

        // assert
        actual.Required.Should().BeTrue();
        actual.HasValue.Should().BeTrue();
    }

    [TestCase("")]
    [TestCase(null)]
    public void Missing_Values_Are_Detected(string value)
    {
        // arrange
        var config = new TestConfigurationMock();
        config._azureEnvironment = value;

        // act
        var sut = new ConfigurationInformation(config);
        var actual = sut.ConfigurationInfo.Single(x => x.Name == "AzureEnvironment");

        // assert
        actual.Required.Should().BeTrue();
        actual.HasValue.Should().BeFalse();
    }

    [Test]
    public void Extraneous_Values_Are_Detected()
    {
        // arrange
        var config = new TestConfigurationMock();
        config._clarityProjectId = "foo";

        // act
        var sut = new ConfigurationInformation(config);
        var actual = sut.ConfigurationInfo.Single(x => x.Name == "ClarityProjectId");

        // assert
        actual.Required.Should().BeFalse();
        actual.Extraneous.Should().BeTrue();
    }

    [Test]
    public void Ignored_Values_Are_Detected()
    {
        // arrange
        var config = new TestConfigurationMock();

        // act
        var sut = new ConfigurationInformation(config);
        var actual = sut.ConfigurationInfo.SingleOrDefault(x => x.Name == "ContentfulDeliveryApiKey");

        // assert
        actual.Should().BeNull();
    }
}
