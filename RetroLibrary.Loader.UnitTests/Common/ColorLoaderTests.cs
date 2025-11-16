using RetroLibrary.Loader.Common;

namespace RetroLibrary.Loader.UnitTests.Common;

public class ColorLoaderTests
{
    [Fact]
    public void GivenKnownColorName_WhenColorFromName_ThenReturnsExpectedColor()
    {
        // Arrange
        var sut = new ColorLoader();
        var colorName = "Red";
        var expectedColor = Microsoft.Xna.Framework.Color.Red;

        // Act
        var result = sut.ColorFromName(colorName, null);

        // Assert
        Assert.Equal(expectedColor, result);
    }

    [Fact]
    public void GivenUnknownColorName_WhenColorFromName_ThenReturnsNull()
    {
        // Arrange
        var sut = new ColorLoader();
        var colorName = "Foobar";
        var expectedColor = Microsoft.Xna.Framework.Color.Red;

        // Act
        var result = sut.ColorFromName(colorName, null);

        // Assert
        Assert.Null(result);
    }
}
