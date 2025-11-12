using Microsoft.Xna.Framework;
using RetroLibrary.Enums;

namespace RetroLibrary.Extensions;

public static class Vector2Extensions
{
    public static Vector2 Align(
        this Vector2 source,
        Rectangle destination,
        HorizontalAlignment horizontalAlignment,
        VerticalAlignment veritcalAlignment)
    {
        var alignedX = horizontalAlignment switch
        {
            HorizontalAlignment.Left => destination.Left,
            HorizontalAlignment.Middle => destination.Left + ((destination.Width - source.X) / 2),
            HorizontalAlignment.Right => destination.Right - source.X,
            _ => source.X
        };

        var alignedY = veritcalAlignment switch
        {
            VerticalAlignment.Top => destination.Top,
            VerticalAlignment.Middle => destination.Top + ((destination.Height - source.Y) / 2),
            VerticalAlignment.Bottom => destination.Bottom - source.Y,
            _ => source.Y
        };

        return new Vector2(alignedX, alignedY);
    }
}
