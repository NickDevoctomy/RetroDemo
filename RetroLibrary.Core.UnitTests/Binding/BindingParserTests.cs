using RetroLibrary.Core.Binding;

namespace RetroLibrary.Core.UnitTests.Binding;

public class BindingParserTests
{
    [Theory]
    [InlineData("{binding Path=Player.Position}", "Player.Position")]
    [InlineData("{binding Path=Player.Position, Something=pop}", "Player.Position")]
    [InlineData("{binding Path=Player.Position, Something={complexArgument Arg1=apple, Arg2=orange}}", "Player.Position")]
    public void GivenBindingString_WhenParse_ThenBindingInfoIsCorrect(
        string bindingString,
        string expectedPath)
    {
        // Arrange
        var test = new object();
        var sut = new BindingParser();

        // Act
        var result = sut.Parse(test, bindingString);

        // Assert
        Assert.Equal(expectedPath, result.Path);
    }

    [Theory]
    [InlineData("")]
    public void GivenInvalidBindingString_WhenParse_ThenArgumentExceptionThrown(string bindingString)
    {
        // Arrange
        var test = new object();
        var sut = new BindingParser();

        // Act / Assert
        Assert.Throws<ArgumentException>(() => sut.Parse(test, bindingString));
    }

    [Theory]
    [InlineData(null)]
    public void GivenInvalidBindingString_WhenParse_ThenArgumentNullExceptionThrown(string bindingString)
    {
        // Arrange
        var test = new object();
        var sut = new BindingParser();

        // Act / Assert
        Assert.Throws<ArgumentNullException>(() => sut.Parse(test, bindingString));
    }

    [Theory]
    [InlineData("{notbinding Path=Player.Position}")]
    [InlineData("notbinding Path=Player.Position")]
    public void GivenMalformedBindingKeyword_WhenParse_ThenFormatExceptionThrown(string bindingString)
    {
        // Arrange
        var test = new object();
        var sut = new BindingParser();

        // Act / Assert
        Assert.Throws<FormatException>(() => sut.Parse(test, bindingString));
    }

    [Theory]
    [InlineData("{binding Player.Position}", "Player.Position")]
    [InlineData("{binding 'Player.Position'}", "Player.Position")]
    [InlineData("{binding \"Player.Position\"}", "Player.Position")]
    [InlineData("{binding Player.Position,}", "Player.Position")]
    public void GivenBindingWithoutExplicitPathArgument_WhenParse_ThenPathIsFirstToken(string bindingString, string expectedPath)
    {
        // Arrange
        var test = new object();
        var sut = new BindingParser();

        // Act
        var result = sut.Parse(test, bindingString);

        // Assert
        Assert.Equal(expectedPath, result.Path);
    }

    [Theory]
    [InlineData("{binding Path='Player.Position'}", "Player.Position")]
    [InlineData("{binding Path=\"Player.Position\"}", "Player.Position")]
    [InlineData("{Binding Path=Player.Position}", "Player.Position")]
    [InlineData("{BINDING Path=Player.Position}", "Player.Position")]
    [InlineData("binding Path=Player.Position", "Player.Position")]
    public void GivenQuotedOrCaseInsensitiveBinding_WhenParse_ThenPathIsUnwrapped(string bindingString, string expectedPath)
    {
        // Arrange
        var test = new object();
        var sut = new BindingParser();

        // Act
        var result = sut.Parse(test, bindingString);

        // Assert
        Assert.Equal(expectedPath, result.Path);
    }

    [Theory]
    [InlineData("{binding Path=Player.Position SomethingElse}", "Player.Position")]
    [InlineData("{binding Path=Player.Position Something=Else}", "Player.Position")]
    [InlineData("{binding Path=Player.Position Foo=Bar Baz=Qux}", "Player.Position")]
    [InlineData("{binding Foo=Bar Player.Position}", "Player.Position")]
    [InlineData("{binding Foo=Bar Player.Position Another=Thing}", "Player.Position")]
    public void GivenAdditionalOrUnknownAttributes_WhenParse_ThenPathRemainsCorrect(string bindingString, string expectedPath)
    {
        // Arrange
        var test = new object();
        var sut = new BindingParser();

        // Act
        var result = sut.Parse(test, bindingString);

        // Assert
        Assert.Equal(expectedPath, result.Path);
    }

    [Theory]
    [InlineData("{binding}")]
    [InlineData("{binding   }")]
    public void GivenEmptyBindingContent_WhenParse_ThenFormatExceptionThrown(string bindingString)
    {
        // Arrange
        var test = new object();
        var sut = new BindingParser();

        // Act & Assert
        Assert.ThrowsAny<FormatException>(() => sut.Parse(test, bindingString));
    }

    [Theory]
    [InlineData("{binding Path=Player.Position,}", "Player.Position")]
    [InlineData("{binding Player.Position, Something=Else}", "Player.Position")]
    public void GivenTrailingCommaOrTokens_WhenParse_ThenPathIsParsed(string bindingString, string expectedPath)
    {
        // Arrange
        var test = new object();
        var sut = new BindingParser();

        // Act
        var result = sut.Parse(test, bindingString);

        // Assert
        Assert.Equal(expectedPath, result.Path);
    }
}
