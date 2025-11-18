using Microsoft.Xna.Framework;

namespace RetroLibrary.Core.Drawing;

/// <summary>
/// Nearest-neighbor implementation of IBlitterService using Point and Rectangle.
/// </summary>
public class BlitterService : IBlitterService
{
    public void BlitExact(
        Color[] source,
        int sourceWidth,
        Rectangle srcRect,
        Color[] dest,
        int destWidth,
        Point destPos)
    {
        if (srcRect.Width <= 0 || srcRect.Height <= 0)
        {
            return;
        }

        var sx = srcRect.X;
        var sy = srcRect.Y;
        var sw = srcRect.Width;
        var sh = srcRect.Height;
        var dx = destPos.X;
        var dy = destPos.Y;

        for (var y = 0; y < sh; y++)
        {
            var srcRow = (sy + y) * sourceWidth;
            var dstRow = (dy + y) * destWidth;
            for (var x = 0; x < sw; x++)
            {
                dest[dstRow + dx + x] = source[srcRow + sx + x];
            }
        }
    }

    public void BlitScaledNearest(
        Color[] source,
        int sourceWidth,
        Rectangle srcRect,
        Color[] dest,
        int destWidth,
        Rectangle destRect)
    {
        if (srcRect.Width <= 0 || srcRect.Height <= 0 || destRect.Width <= 0 || destRect.Height <= 0)
        {
            return;
        }

        var sx = srcRect.X;
        var sy = srcRect.Y;
        var sw = srcRect.Width;
        var sh = srcRect.Height;
        var dx = destRect.X;
        var dy = destRect.Y;
        var dw = destRect.Width;
        var dh = destRect.Height;

        for (var y = 0; y < dh; y++)
        {
            var srcY = sy + (int)(y * (sh / (float)dh));
            var srcRow = srcY * sourceWidth;
            var dstRow = (dy + y) * destWidth;
            for (var x = 0; x < dw; x++)
            {
                var srcX = sx + (int)(x * (sw / (float)dw));
                dest[dstRow + dx + x] = source[srcRow + srcX];
            }
        }
    }
}