using Childrens_Social_Care_CPD;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Childrens_Social_Care_CPD_Tests;

[NonParallelizable]
public class ApplicationConfigurationTests
{
    private const string Value = "foo";

    private void ClearEnvironmentVariables()
    {
        Environment.SetEnvironmentVariable("CPD_INSTRUMENTATION_CONNECTIONSTRING", null);
        Environment.SetEnvironmentVariable("CPD_AZURE_ENVIRONMENT", null);
        Environment.SetEnvironmentVariable("CPD_CLARITY", null);
        Environment.SetEnvironmentVariable("CPD_DELIVERY_KEY", null);
        Environment.SetEnvironmentVariable("CPD_CONTENTFUL_ENVIRONMENT", null);
        Environment.SetEnvironmentVariable("CPD_PREVIEW_KEY", null);
        Environment.SetEnvironmentVariable("CPD_SPACE_ID", null);
        Environment.SetEnvironmentVariable("VCS-REF", null);
        Environment.SetEnvironmentVariable("CPD_GOOGLEANALYTICSTAG", null);
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
        var actual = sut.AzureEnvironment;

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
        var actual = sut.ClarityProjectId;

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
        var actual = sut.ContentfulDeliveryApiKey;

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
        var actual = sut.ContentfulEnvironment;

        // assert
        actual.Should().Be(Value);
    }

    [Test]
    public void Returns_ContentfulPreviewHost_Value()
    {
        // arrange
        var value = "preview.contentful.com";
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.ContentfulPreviewHost;

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
        var actual = sut.ContentfulPreviewId;

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
        var actual = sut.ContentfulSpaceId;

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
        var actual = sut.GoogleTagManagerKey;

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
        var actual = sut.AppInsightsConnectionString;

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
        var actual = sut.GitHash;

        // assert
        actual.Should().Be(Value);
    }


    [Test]
    public void Returns_AzureEnvironment_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.AzureEnvironment;

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void Returns_ClarityProjectId_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.ClarityProjectId;

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void Returns_ContentfulDeliveryApiKey_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.ContentfulDeliveryApiKey;

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void Returns_ContentfulEnvironment_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.ContentfulEnvironment;

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void Returns_ContentfulPreviewId_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.ContentfulPreviewId;

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void Returns_ContentfulSpaceId_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.ContentfulSpaceId;

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void Returns_GoogleTagManagerKey_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.GoogleTagManagerKey;

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void Returns_AppInsightsConnectionString_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.AppInsightsConnectionString;

        // assert
        actual.Should().Be(string.Empty);
    }

    [Test]
    public void Returns_GitHash_Default_Value()
    {
        // arrange
        var sut = new ApplicationConfiguration();

        // act
        var actual = sut.GitHash;

        // assert
        actual.Should().Be(string.Empty);
    }
}
