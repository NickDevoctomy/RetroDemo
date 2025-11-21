using Microsoft.Xna.Framework.Graphics;

namespace RetroLibrary.Core.Drawing;

public interface IRetroTexture2D
{
    public void Draw(
        int gradientWidth,
        int gradientHeight,
        SpriteBatch spriteBatch,
        Microsoft.Xna.Framework.Rectangle sourceRectangle,
        Microsoft.Xna.Framework.Rectangle destinationRectangle);
}