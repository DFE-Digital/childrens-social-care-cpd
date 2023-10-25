using NUnit.Framework;
using Childrens_Social_Care_CPD.Configuration;
using FluentAssertions;
using System.Linq;

namespace Childrens_Social_Care_CPD_Tests.Configuration;

public class RequiredForEnvironmentTests
{
    private class TestConfiguration
    {
        public string PropertyHasNoRules { get; set; }

        [RequiredForEnvironment("dev")]
        public string PropertyHasSingleEnv { get; set; }

        [RequiredForEnvironment("*")]
        public string PropertyHasAllEnv { get; set; }

        [RequiredForEnvironment("dev")]
        [RequiredForEnvironment("test")]
        [RequiredForEnvironment("prod")]
        public string PropertyHasMultipleEnv { get; set; }

        [RequiredForEnvironment("dev", Hidden = false)]
        [RequiredForEnvironment("*")]
        public string PropertyHasEnvironmentOverrideValue { get; set; }
    }

    [Test]
    public void No_Rule_Property_Is_Ignored()
    {
        // arrange
        var config = new TestConfiguration();

        // act
        var sut = config.RulesForEnvironment("dev");
        var actual = sut.Where(x => x.Key.Name == "PropertyHasNoRules");

        // assert
        actual.Should().HaveCount(0);
    }

    [Test]
    public void Single_Environment_Property_Returns_Rule_For_Correct_Environment()
    {
        // arrange
        var config = new TestConfiguration();

        // act
        var sut = config.RulesForEnvironment("dev");
        var actual = sut.SingleOrDefault(x => x.Key.Name == "PropertyHasSingleEnv");

        // assert
        actual.Value.Should().NotBeNull();
    }

    [Test]
    public void Single_Environment_Property_Returns_No_Rule_For_Wrong_Environment()
    {
        // arrange
        var config = new TestConfiguration();

        // act
        var sut = config.RulesForEnvironment(" ");
        var actual = sut.SingleOrDefault(x => x.Key.Name == "PropertyHasSingleEnv");

        // assert
        actual.Value.Should().BeNull();
    }

    [Test]
    public void Asterisk_Matches_Any_Environment()
    {
        // arrange
        var config = new TestConfiguration();

        // act
        var sut = config.RulesForEnvironment("test");
        var actual = sut.SingleOrDefault(x => x.Key.Name == "PropertyHasAllEnv");

        // assert
        actual.Value.Should().NotBeNull();
    }

    [TestCase("dev")]
    [TestCase("test")]
    [TestCase("prod")]
    public void Multiple_Rules_For_Different_Environments_For_Same_Property_Return_Single_Rule(string env)
    {
        // arrange
        var config = new TestConfiguration();

        // act
        var sut = config.RulesForEnvironment(env);
        var actual = sut.SingleOrDefault(x => x.Key.Name == "PropertyHasMultipleEnv");

        // assert
        actual.Value.Should().NotBeNull();
    }

    [Test]
    public void All_Environment_Matcher_Can_Be_Overidden()
    {
        // arrange
        var config = new TestConfiguration();

        // act
        var sut = config.RulesForEnvironment("dev");
        var actual = sut.SingleOrDefault(x => x.Key.Name == "PropertyHasEnvironmentOverrideValue");

        // assert
        actual.Value.Should().NotBeNull();
        actual.Value.Environment.Should().Be("dev");
        actual.Value.Hidden.Should().Be(false);
    }

    [Test]
    public void Override_For_Non_Matching_Environment_Does_Not_Override_Catchall_Rule()
    {
        // arrange
        var config = new TestConfiguration();

        // act
        var sut = config.RulesForEnvironment("text");
        var actual = sut.SingleOrDefault(x => x.Key.Name == "PropertyHasEnvironmentOverrideValue");

        // assert
        actual.Value.Should().NotBeNull();
        actual.Value.Environment.Should().Be("*");
        actual.Value.Hidden.Should().Be(true);
    }
}
