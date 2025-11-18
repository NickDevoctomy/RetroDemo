using Microsoft.Xna.Framework;

namespace RetroLibrary.Core.Drawing;

public interface IBlitterService
{
    /// <summary>
    /// Copies a rectangular region exactly (no scaling) from source to destination.
    /// </summary>
    /// <param name="source">Source pixel array.</param>
    /// <param name="sourceWidth">Width of the source texture.</param>
    /// <param name="srcRect">Source rectangle to copy.</param>
    /// <param name="dest">Destination pixel array.</param>
    /// <param name="destWidth">Width of the destination texture.</param>
    /// <param name="destPos">Destination position (top-left) to place the copied region.</param>
    void BlitExact(
        Color[] source,
        int sourceWidth,
        Rectangle srcRect,
        Color[] dest,
        int destWidth,
        Point destPos);

    /// <summary>
    /// Scales a rectangular region from source into a destination rectangle using nearest-neighbor.
    /// </summary>
    /// <param name="source">Source pixel array.</param>
    /// <param name="sourceWidth">Width of the source texture.</param>
    /// <param name="srcRect">Source rectangle to sample from.</param>
    /// <param name="dest">Destination pixel array.</param>
    /// <param name="destWidth">Width of the destination texture.</param>
    /// <param name="destRect">Destination rectangle to write to.</param>
    void BlitScaledNearest(
        Color[] source,
        int sourceWidth,
        Rectangle srcRect,
        Color[] dest,
        int destWidth,
        Rectangle destRect);
}
