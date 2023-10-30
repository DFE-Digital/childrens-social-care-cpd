using Childrens_Social_Care_CPD.Configuration;
using Contentful.Core.Models.Management;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Childrens_Social_Care_CPD_Tests.Configuration;

[NonParallelizable]
public class ApplicationConfigurationTests
{
    private const string Value = "foo";

    private static void ClearEnvironmentVariables()
    {
        Environment.SetEnvironmentVariable("VCS-TAG", null);
        Environment.SetEnvironmentVariable("CPD_INSTRUMENTATION_CONNECTIONSTRING", null);
        Environment.SetEnvironmentVariable("CPD_AZURE_ENVIRONMENT", null);
        Environment.SetEnvironmentVariable("CPD_CLARITY", null);
        Environment.SetEnvironmentVariable("CPD_DELIVERY_KEY", null);
        Environment.SetEnvironmentVariable("CPD_CONTENTFUL_ENVIRONMENT", null);
        Environment.SetEnvironmentVariable("CPD_PREVIEW_KEY", null);
        Environment.SetEnvironmentVariable("CPD_SPACE_ID", null);
        Environment.SetEnvironmentVariable("VCS-REF", null);
        Environment.SetEnvironmentVariable("CPD_GOOGLEANALYTICSTAG", null);
        Environment.SetEnvironmentVariable("CPD_DISABLE_SECURE_COOKIES", null);
        Environment.SetEnvironmentVariable("CPD_FEATURE_POLLING_INTERVAL", null);
    }

    [SetUp]
    public void Setup()
    {
        ClearEnvironmentVariables();
    }

    [TearDown]
    public void Teardown()
    {
        ClearEnvironmentVariables();
    }

    [Test]
    public void Returns_AzureEnvironment_Value()
    {
        // arrange
        Environment.SetEnvironmentVariable("CPD_AZURE_ENVIRONMENT", Value);
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.AzureEnvironment.Value;

        // assert
        actual.Should().Be(Value);
    }

    [Test]
    public void Returns_ClarityProjectId_Value()
    {
        // arrange
        Environment.SetEnvironmentVariable("CPD_CLARITY", Value);
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.ClarityProjectId.Value;

        // assert
        actual.Should().Be(Value);
    }

    [Test]
    public void Returns_ContentfulDeliveryApiKey_Value()
    {
        // arrange
        var Value = "foo";
        Environment.SetEnvironmentVariable("CPD_DELIVERY_KEY", Value);
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.ContentfulDeliveryApiKey.Value;

        // assert
        actual.Should().Be(Value);
    }

    [Test]
    public void Returns_ContentfulEnvironment_Value()
    {
        // arrange
        Environment.SetEnvironmentVariable("CPD_CONTENTFUL_ENVIRONMENT", Value);
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.ContentfulEnvironment.Value;

        // assert
        actual.Should().Be(Value);
    }

    [Test]
    public void Returns_ContentfulGraphqlConnectionString_Value()
    {
        // arrange
        Environment.SetEnvironmentVariable("CPD_SPACE_ID", Value);
        Environment.SetEnvironmentVariable("CPD_CONTENTFUL_ENVIRONMENT", Value);
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.ContentfulGraphqlConnectionString.Value;

        // assert
        actual.Should().Be($"https://graphql.contentful.com/content/v1/spaces/{Value}/environments/{Value}");
    }

    [Test]
    public void Returns_ContentfulPreviewHost_Value()
    {
        // arrange
        var value = "preview.contentful.com";
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.ContentfulPreviewHost.Value;

        // assert
        actual.Should().Be(value);
    }

    [Test]
    public void Returns_ContentfulPreviewId_Value()
    {
        // arrange
        Environment.SetEnvironmentVariable("CPD_PREVIEW_KEY", Value);
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.ContentfulPreviewId.Value;

        // assert
        actual.Should().Be(Value);
    }

    [Test]
    public void Returns_ContentfulSpaceId_Value()
    {
        // arrange
        Environment.SetEnvironmentVariable("CPD_SPACE_ID", Value);
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.ContentfulSpaceId.Value;

        // assert
        actual.Should().Be(Value);
    }

    [Test]
    public void Returns_GoogleTagManagerKey_Value()
    {
        // arrange
        Environment.SetEnvironmentVariable("CPD_GOOGLEANALYTICSTAG", Value);
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.GoogleTagManagerKey.Value;

        // assert
        actual.Should().Be(Value);
    }

    [Test]
    public void Returns_AppInsightsConnectionString_Value()
    {
        // arrange
        Environment.SetEnvironmentVariable("CPD_INSTRUMENTATION_CONNECTIONSTRING", Value);
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.AppInsightsConnectionString.Value;

        // assert
        actual.Should().Be(Value);
    }

    [Test]
    public void Returns_GitHash_Value()
    {
        // arrange
        Environment.SetEnvironmentVariable("VCS-REF", Value);
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.GitHash.Value;

        // assert
        actual.Should().Be(Value);
    }

    [Test]
    public void Returns_DisableSecureCookies_Value()
    {
        // arrange
        Environment.SetEnvironmentVariable("CPD_DISABLE_SECURE_COOKIES", "false");
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.DisableSecureCookies.Value;

        // assert
        actual.Should().Be(false);
    }

    [Test]
    public void Returns_AppVersionEnvironment_Value()
    {
        // arrange
        Environment.SetEnvironmentVariable("VCS-TAG", Value);
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.AppVersion.Value;

        // assert
        actual.Should().Be(Value);
    }

    [Test]
    public void Returns_FeaturePollInterval_Value()
    {
        // arrange
        Environment.SetEnvironmentVariable("CPD_FEATURE_POLLING_INTERVAL", "10000");
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.FeaturePollingInterval.Value;

        // assert
        actual.Should().Be(10000);
    }

    [Test]
    public void Returns_FeaturePollInterval_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.FeaturePollingInterval.Value;

        // assert
        actual.Should().Be(0);
    }

    [Test]
    public void Returns_AzureEnvironment_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.AzureEnvironment.Value;

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void Returns_ClarityProjectId_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.ClarityProjectId.Value;

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void Returns_ContentfulDeliveryApiKey_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.ContentfulDeliveryApiKey.Value;

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void Returns_ContentfulEnvironment_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.ContentfulEnvironment.Value;

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void Returns_ContentfulPreviewId_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.ContentfulPreviewId.Value;

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void Returns_ContentfulSpaceId_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.ContentfulSpaceId.Value;

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void Returns_GoogleTagManagerKey_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.GoogleTagManagerKey.Value;

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void Returns_AppInsightsConnectionString_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.AppInsightsConnectionString.Value;

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void Returns_GitHash_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.GitHash.Value;

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void Returns_DisableSecureCookies_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.DisableSecureCookies.Value;

        // assert
        actual.Should().Be(false);
    }

    [Test]
    public void Returns_AppVersionEnvironment_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.AppVersion.Value;

        // assert
        actual.Should().Be(string.Empty);
    }
}
