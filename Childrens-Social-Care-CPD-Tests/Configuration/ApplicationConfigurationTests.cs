﻿using Childrens_Social_Care_CPD.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Childrens_Social_Care_CPD_Tests.Configuration;

[NonParallelizable]
public class ApplicationConfigurationTests
{
    private IConfiguration _configuration;
    private ApplicationConfiguration _sut;

    [SetUp]
    public void Setup()
    {
        _configuration = Substitute.For<IConfiguration>();
        _sut = new ApplicationConfiguration(_configuration);
    }

    [Test]
    public void Returns_ContentfulGraphqlConnectionString_Value()
    {
        // arrange
        _configuration["CPD_SPACE_ID"].Returns("foo");
        _configuration["CPD_CONTENTFUL_ENVIRONMENT"].Returns("foo");

        // act
        var actual = _sut.ContentfulGraphqlConnectionString.Value;

        // assert
        actual.Should().Be($"https://graphql.contentful.com/content/v1/spaces/foo/environments/foo");
    }

    [Test]
    public void Returns_ContentfulPreviewHost_Value()
    {
        // arrange
        var value = "preview.contentful.com";

        // act
        var actual = _sut.ContentfulPreviewHost.Value;

        // assert
        actual.Should().Be(value);
    }

    public static IEnumerable<(string, Func<ApplicationConfiguration, object>, string, object)> TestCases
    {
        get
        {
            // Set values
            yield return ("CPD_AZURE_ENVIRONMENT", (ApplicationConfiguration sut) => sut.AzureEnvironment, "foo", "foo");
            yield return ("CPD_CLARITY", (ApplicationConfiguration sut) => sut.ClarityProjectId, "foo", "foo");
            yield return ("CPD_DELIVERY_KEY", (ApplicationConfiguration sut) => sut.ContentfulDeliveryApiKey, "foo", "foo");
            yield return ("CPD_CONTENTFUL_ENVIRONMENT", (ApplicationConfiguration sut) => sut.ContentfulEnvironment, "foo", "foo");
            yield return ("CPD_PREVIEW_KEY", (ApplicationConfiguration sut) => sut.ContentfulPreviewId, "foo", "foo");
            yield return ("CPD_SPACE_ID", (ApplicationConfiguration sut) => sut.ContentfulSpaceId, "foo", "foo");
            yield return ("CPD_GOOGLEANALYTICSTAG", (ApplicationConfiguration sut) => sut.GoogleTagManagerKey, "foo", "foo");
            yield return ("CPD_INSTRUMENTATION_CONNECTIONSTRING", (ApplicationConfiguration sut) => sut.AppInsightsConnectionString, "foo", "foo");
            yield return ("VCS-REF", (ApplicationConfiguration sut) => sut.GitHash, "foo", "foo");
            yield return ("VCS-TAG", (ApplicationConfiguration sut) => sut.AppVersion, "foo", "foo");
            yield return ("CPD_DISABLE_SECURE_COOKIES", (ApplicationConfiguration sut) => sut.DisableSecureCookies, "true", true);
            yield return ("CPD_FEATURE_POLLING_INTERVAL", (ApplicationConfiguration sut) => sut.FeaturePollingInterval, "10", 10);
            yield return ("CPD_SEARCH_API_KEY", (ApplicationConfiguration sut) => sut.SearchApiKey, "foo", "foo");
            yield return ("CPD_SEARCH_END_POINT", (ApplicationConfiguration sut) => sut.SearchEndpoint, "foo", "foo");
            yield return ("CPD_SEARCH_INDEX_NAME", (ApplicationConfiguration sut) => sut.SearchIndexName, "foo", "foo");
            // Default values
            yield return ("CPD_AZURE_ENVIRONMENT", (ApplicationConfiguration sut) => sut.AzureEnvironment, null, "");
            yield return ("CPD_CLARITY", (ApplicationConfiguration sut) => sut.ClarityProjectId, null, "");
            yield return ("CPD_DELIVERY_KEY", (ApplicationConfiguration sut) => sut.ContentfulDeliveryApiKey, null, "");
            yield return ("CPD_CONTENTFUL_ENVIRONMENT", (ApplicationConfiguration sut) => sut.ContentfulEnvironment, null, "");
            yield return ("CPD_PREVIEW_KEY", (ApplicationConfiguration sut) => sut.ContentfulPreviewId, null, "");
            yield return ("CPD_SPACE_ID", (ApplicationConfiguration sut) => sut.ContentfulSpaceId, null, "");
            yield return ("CPD_GOOGLEANALYTICSTAG", (ApplicationConfiguration sut) => sut.GoogleTagManagerKey, null, "");
            yield return ("CPD_INSTRUMENTATION_CONNECTIONSTRING", (ApplicationConfiguration sut) => sut.AppInsightsConnectionString, null, "");
            yield return ("VCS-REF", (ApplicationConfiguration sut) => sut.GitHash, null, "");
            yield return ("VCS-TAG", (ApplicationConfiguration sut) => sut.AppVersion, null, "");
            yield return ("CPD_DISABLE_SECURE_COOKIES", (ApplicationConfiguration sut) => sut.DisableSecureCookies, null, false);
            yield return ("CPD_FEATURE_POLLING_INTERVAL", (ApplicationConfiguration sut) => sut.FeaturePollingInterval, null, 0);
            yield return ("CPD_SEARCH_API_KEY", (ApplicationConfiguration sut) => sut.SearchApiKey, null, "");
            yield return ("CPD_SEARCH_END_POINT", (ApplicationConfiguration sut) => sut.SearchEndpoint, null, "");
            yield return ("CPD_SEARCH_INDEX_NAME", (ApplicationConfiguration sut) => sut.SearchIndexName, null, "");
        }
    }

    [TestCaseSource(nameof(TestCases))]
    public void Returns_AzureEnvironment_Value((string key, Func<ApplicationConfiguration, object> property, string value, object expected) values)
    {
        // arrange
        _configuration[values.key].Returns(values.value);
        var targetValue = values.property(_sut);
        var valueProperty = targetValue.GetType().GetProperty("Value");

        // act
        var actual = valueProperty.GetValue(targetValue);

        // assert
        actual.Should().Be(values.expected);
    }
}
