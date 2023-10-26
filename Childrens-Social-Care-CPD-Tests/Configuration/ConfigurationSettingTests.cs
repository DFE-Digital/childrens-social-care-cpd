using Childrens_Social_Care_CPD.Configuration;
using FluentAssertions;
using NUnit.Framework;

namespace Childrens_Social_Care_CPD_Tests.Configuration;

public class ConfigurationSettingTests
{
    [Test]
    public void Default_Value_Is_Set()
    {
        // arrange
        var defaultValue = new object();

        // act
        var sut = new ConfigurationSetting<object>(() => " ", x => new object(), defaultValue);

        // assert
        sut.Value.Should().Be(defaultValue);
    }

    [Test]
    public void Value_Calls_Getter()
    {
        // arrange
        var input = "x";
        var value = new object();
        var passedValue = string.Empty;

        // act
        var sut = new ConfigurationSetting<object>(() => input, x => {
            passedValue = input;
            return value;
        });

        // accessing Value causes the parser to be called
        sut.Value.Should().NotBeNull();

        // assert
        passedValue.Should().BeEquivalentTo(input);
    }

    [Test]
    public void Value_Calls_Parser()
    {
        // arrange
        var value = new object();

        // act
        var sut = new ConfigurationSetting<object>(() => "x", x => value);

        // assert
        sut.Value.Should().Be(value);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void IsSet_Returns_False_For_Invalid_Input(string value)
    {
        // act
        var sut = new ConfigurationSetting<object>(() => value, x => new object());

        // assert
        sut.IsSet.Should().BeFalse();
    }

    [Test]
    public void IsSet_Returns_True_For_Valid_Input()
    {
        // act
        var sut = new ConfigurationSetting<object>(() => "x", x => new object());

        // assert
        sut.IsSet.Should().BeTrue();
    }

    [Test]
    public void ConfigurationSetting_ToString_Returns_Value_Representation()
    {
        // arrange
        var value = new object();

        // act
        var sut = new ConfigurationSetting<object>(() => "x", x => "foo");

        // assert
        sut.ToString().Should().Be("foo");
    }

    [Test]
    public void ConfigurationSetting_ToString_Should_Not_Return_Null()
    {
        // arrange
        var value = new object();

        // act
        var sut = new ConfigurationSetting<object>(() => "x", x => null);

        // assert
        sut.ToString().Should().Be(string.Empty);
    }
}