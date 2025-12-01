using Microsoft.Xna.Framework;
using RetroLibrary.Core.Enums;

namespace RetroLibrary.Core.Extensions;

public static class RectangleExtensions
{
    public static Rectangle GetBounds(
        this Rectangle source,
        Rectangle margins,
        Rectangle padding)
    {
        int x = source.X + margins.X + padding.X;
        int y = source.Y + margins.Y + padding.Y;
        int width = Math.Max(0, source.Width - padding.X - padding.Width);
        int height = Math.Max(0, source.Height - padding.Y - padding.Height);

        return new Rectangle(x, y, width, height);
    }

    public static Rectangle Align(
        this Rectangle source,
        Rectangle destination,
        HorizontalAlignment horizontalAlignment,
        VerticalAlignment veritcalAlignment)
    {
        var alignedX = horizontalAlignment switch
        {
            HorizontalAlignment.Left => destination.Left,
            HorizontalAlignment.Middle => destination.Left + ((destination.Width - source.Width) / 2),
            HorizontalAlignment.Right => destination.Right - source.Width,
            _ => source.X
        };

        var alignedY = veritcalAlignment switch
        {
            VerticalAlignment.Top => destination.Top,
            VerticalAlignment.Middle => destination.Top + ((destination.Height - source.Height) / 2),
            VerticalAlignment.Bottom => destination.Bottom - source.Height,
            _ => source.Y
        };

        return new Rectangle(alignedX, alignedY, source.Width, source.Height);
    }
}