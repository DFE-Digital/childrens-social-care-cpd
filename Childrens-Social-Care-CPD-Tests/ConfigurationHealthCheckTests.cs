using Castle.Core.Logging;
using Childrens_Social_Care_CPD;
using Childrens_Social_Care_CPD.Configuration;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.Extensions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Childrens_Social_Care_CPD_Tests;

public class ConfigurationHealthCheckTests
{
    private ILogger<ConfigurationHealthCheck> _logger;
    private IApplicationConfiguration _applicationConfiguration;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<ConfigurationHealthCheck>>();
        _applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        _applicationConfiguration.ReturnsForAll("foo");
    }

    [Test]
    public async Task Fails_When_Disable_Cookies_Is_True()
    {
        // arrange
        _applicationConfiguration.DisableSecureCookies.Returns(true);
        var sut = new ConfigurationHealthCheck(_logger, _applicationConfiguration);

        // act
        var result = await sut.CheckHealthAsync(null, default);

        // assert
        result.Status.Should().Be(HealthStatus.Unhealthy);
    }

    [Test]
    public async Task Fails_When_AppInsightsConnectionString_Is_Not_Set()
    {
        // arrange
        _applicationConfiguration.AppInsightsConnectionString.Returns(string.Empty);
        var sut = new ConfigurationHealthCheck(_logger, _applicationConfiguration);

        // act
        var result = await sut.CheckHealthAsync(null, default);

        // assert
        result.Status.Should().Be(HealthStatus.Unhealthy);
    }

    [Test]
    public async Task Fails_When_AppVersion_Is_Not_Set()
    {
        // arrange
        _applicationConfiguration.AppVersion.Returns(string.Empty);
        var sut = new ConfigurationHealthCheck(_logger, _applicationConfiguration);

        // act
        var result = await sut.CheckHealthAsync(null, default);

        // assert
        result.Status.Should().Be(HealthStatus.Unhealthy);
    }

    [Test]
    public async Task Fails_When_AzureEnvironment_Is_Not_Set()
    {
        // arrange
        _applicationConfiguration.AzureEnvironment.Returns(string.Empty);
        var sut = new ConfigurationHealthCheck(_logger, _applicationConfiguration);

        // act
        var result = await sut.CheckHealthAsync(null, default);

        // assert
        result.Status.Should().Be(HealthStatus.Unhealthy);
    }

    [Test]
    public async Task Fails_When_ClarityProjectId_Is_Not_Set()
    {
        // arrange
        _applicationConfiguration.ClarityProjectId.Returns(string.Empty);
        var sut = new ConfigurationHealthCheck(_logger, _applicationConfiguration);

        // act
        var result = await sut.CheckHealthAsync(null, default);

        // assert
        result.Status.Should().Be(HealthStatus.Unhealthy);
    }

    [Test]
    public async Task Fails_When_ContentfulDeliveryApiKey_Is_Not_Set()
    {
        // arrange
        _applicationConfiguration.ContentfulDeliveryApiKey.Returns(string.Empty);
        var sut = new ConfigurationHealthCheck(_logger, _applicationConfiguration);

        // act
        var result = await sut.CheckHealthAsync(null, default);

        // assert
        result.Status.Should().Be(HealthStatus.Unhealthy);
    }

    [Test]
    public async Task Fails_When_ContentfulEnvironment_Is_Not_Set()
    {
        // arrange
        _applicationConfiguration.ContentfulEnvironment.Returns(string.Empty);
        var sut = new ConfigurationHealthCheck(_logger, _applicationConfiguration);

        // act
        var result = await sut.CheckHealthAsync(null, default);

        // assert
        result.Status.Should().Be(HealthStatus.Unhealthy);
    }

    [Test]
    public async Task Fails_When_ContentfulSpaceId_Is_Not_Set()
    {
        // arrange
        _applicationConfiguration.ContentfulSpaceId.Returns(string.Empty);
        var sut = new ConfigurationHealthCheck(_logger, _applicationConfiguration);

        // act
        var result = await sut.CheckHealthAsync(null, default);

        // assert
        result.Status.Should().Be(HealthStatus.Unhealthy);
    }

    [Test]
    public async Task Fails_When_GoogleTagManagerKey_Is_Not_Set()
    {
        // arrange
        _applicationConfiguration.GoogleTagManagerKey.Returns(string.Empty);
        var sut = new ConfigurationHealthCheck(_logger, _applicationConfiguration);

        // act
        var result = await sut.CheckHealthAsync(null, default);

        // assert
        result.Status.Should().Be(HealthStatus.Unhealthy);
    }
}
