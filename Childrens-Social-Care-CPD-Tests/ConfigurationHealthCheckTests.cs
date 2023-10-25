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
    }

    [Test]
    public async Task Passes_When_All_Values_Set_And_Cookies_Are_Secured()
    {
        // arrange
        _applicationConfiguration.ReturnsForAll("foo");
        var sut = new ConfigurationHealthCheck(_logger, _applicationConfiguration);

        // act
        var result = await sut.CheckHealthAsync(null, default);

        // assert
        result.Status.Should().Be(HealthStatus.Healthy);
    }

    [Test]
    public async Task Fails_When_Disable_Cookies_Is_True()
    {
        // arrange
        _applicationConfiguration.ReturnsForAll("foo");
        _applicationConfiguration.DisableSecureCookies.Returns(true);
        var sut = new ConfigurationHealthCheck(_logger, _applicationConfiguration);

        // act
        var result = await sut.CheckHealthAsync(null, default);

        // assert
        result.Status.Should().Be(HealthStatus.Unhealthy);
    }

    [Test]
    public async Task Fails_When_Required_Value_Is_Missing()
    {
        // arrange
        var sut = new ConfigurationHealthCheck(_logger, _applicationConfiguration);

        // act
        var result = await sut.CheckHealthAsync(null, default);

        // assert
        result.Status.Should().Be(HealthStatus.Unhealthy);
    }
}
