using NUnit.Framework;
using Childrens_Social_Care_CPD.Configuration;
using FluentAssertions;
using System.Linq;
using NSubstitute;

namespace Childrens_Social_Care_CPD_Tests.Configuration;

public partial class ConfigurationInformationTests
{
    private IApplicationConfiguration _applicationConfiguration;

    [SetUp]
    public void Setup()
    { 
        _applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        _applicationConfiguration.AzureEnvironment.Returns("dev");
    }

    [Test]
    public void Required_Values_Are_Detected()
    {
        // arrange
        _applicationConfiguration.AppVersion.Returns("foo");
        
        // act
        var sut = new ConfigurationInformation(_applicationConfiguration);
        var actual = sut.ConfigurationInfo.Single(x => x.Name == "AppVersion");

        // assert
        actual.Required.Should().BeTrue();
        actual.HasValue.Should().BeTrue();
    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void Missing_Values_Are_Detected(string value)
    {
        // arrange
        _applicationConfiguration.AppVersion.Returns(value);

        // act
        var sut = new ConfigurationInformation(_applicationConfiguration);
        var actual = sut.ConfigurationInfo.Single(x => x.Name == "AppVersion");

        // assert
        actual.Required.Should().BeTrue();
        actual.HasValue.Should().BeFalse();
    }

    [Test]
    public void Extraneous_Values_Are_Detected()
    {
        // arrange
        _applicationConfiguration.ClarityProjectId.Returns("foo");

        // act
        var sut = new ConfigurationInformation(_applicationConfiguration);
        var actual = sut.ConfigurationInfo.Single(x => x.Name == "ClarityProjectId");

        // assert
        actual.Required.Should().BeFalse();
        actual.Extraneous.Should().BeTrue();
    }

    [Test]
    public void Ignored_Values_Are_Detected()
    {
        // act
        var sut = new ConfigurationInformation(_applicationConfiguration);
        var actual = sut.ConfigurationInfo.SingleOrDefault(x => x.Name == "ContentfulPreviewHost");

        // assert
        actual.Should().BeNull();
    }

    [Test]
    public void Sensitive_Values_Are_Obfuscated()
    {
        // arrange
        var value = "sensitive value";
        _applicationConfiguration.AzureEnvironment.Returns(ApplicationEnvironment.Production);
        _applicationConfiguration.AppInsightsConnectionString.Returns(value);

        // act
        var sut = new ConfigurationInformation(_applicationConfiguration);
        var actual = sut.ConfigurationInfo.Single(x => x.Name == "AppInsightsConnectionString");

        // assert
        actual.Value.Should().NotBe(value);
    }
}
