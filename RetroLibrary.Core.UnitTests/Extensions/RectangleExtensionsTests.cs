using Microsoft.Xna.Framework;
using RetroLibrary.Core.Enums;
using RetroLibrary.Core.Extensions;

namespace RetroLibrary.Core.UnitTests.Extensions;

public class RectangleExtensionsTests
{
    public static IEnumerable<object[]> GetBoundsCases => new List<object[]>
    {
        new object[]
        {
            new Rectangle(100, 100, 100, 100),  // Source
            new Rectangle(8, 8, 0, 0),          // Margins
            new Rectangle(2, 2, 2, 2),          // Padding
            new Rectangle(110, 110, 96, 96)     // Expected
        },
        new object[]
        {
            new Rectangle(0, 0, 50, 40),
            new Rectangle(0, 0, 0, 0),
            new Rectangle(0, 0, 0, 0),
            new Rectangle(0, 0, 50, 40)
        },
        new object[]
        {
            new Rectangle(20, 30, 50, 40),
            new Rectangle(0, 0, 10, 5),
            new Rectangle(3, 4, 0, 0),
            new Rectangle(23, 34, 47, 36)
        },
        new object[]
        {
            new Rectangle(10, 10, 10, 20),
            new Rectangle(1, 2, 0, 0),
            new Rectangle(6, 2, 5, 3),
            new Rectangle(17, 14, 0, 15)
        },
        new object[]
        {
            new Rectangle(5, 5, 100, 50),
            new Rectangle(7, 9, 11, 13),
            new Rectangle(4, 6, 4, 6),
            new Rectangle(16, 20, 92, 38)
        }
    };

    [Theory]
    [MemberData(nameof(GetBoundsCases))]
    public void GivenRectangleMarginsAndPadding_WhenGetBounds_ThenReturnsExpectedBounds(
        Rectangle source,
        Rectangle margins,
        Rectangle padding,
        Rectangle expected)
    {
        // Act
        var result = source.GetBounds(margins, padding);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GivenExcessiveMarginsAndPadding_WhenGetBounds_ThenWidthAndHeightClampedToZero()
    {
        // Arrange
        var source = new Rectangle(0, 0, 10, 10);
        var margins = new Rectangle(8, 8, 8, 8);
        var padding = new Rectangle(5, 5, 5, 5);

        // Act
        var result = source.GetBounds(margins, padding);

        // Assert
        Assert.Equal(0, result.Width);
        Assert.Equal(0, result.Height);
        Assert.Equal(new Point(source.X + margins.X + padding.X, source.Y + margins.Y + padding.Y), result.Location);
    }

    [Fact]
    public void GivenDestinationAndSource_WhenAlignLeftTop_ThenAlignsToTopLeft()
    {
        // Arrange
        var destination = new Rectangle(10, 10, 100, 50);
        var source = new Rectangle(0, 0, 20, 10);
        var expected = new Rectangle(destination.Left, destination.Top, source.Width, source.Height);

        // Act
        var result = source.Align(destination, HorizontalAlignment.Left, VerticalAlignment.Top);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GivenDestinationAndSource_WhenAlignMiddleMiddle_ThenCentersWithinDestination()
    {
        // Arrange
        var destination = new Rectangle(10, 10, 100, 50);
        var source = new Rectangle(0, 0, 20, 10);
        var expected = new Rectangle(50, 30, 20, 10);

        // Act
        var result = source.Align(destination, HorizontalAlignment.Middle, VerticalAlignment.Middle);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GivenDestinationAndSource_WhenAlignRightBottom_ThenAlignsToBottomRight()
    {
        // Arrange
        var destination = new Rectangle(10, 10, 100, 50);
        var source = new Rectangle(0, 0, 20, 10);
        var expected = new Rectangle(90, 50, 20, 10);

        // Act
        var result = source.Align(destination, HorizontalAlignment.Right, VerticalAlignment.Bottom);

        // Assert
        Assert.Equal(expected, result);
    }
}
