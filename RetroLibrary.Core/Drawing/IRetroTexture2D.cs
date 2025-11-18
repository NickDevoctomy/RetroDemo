using Microsoft.Xna.Framework.Graphics;

namespace RetroLibrary.Core.Drawing;

public interface IRetroTexture2D
{
    public void Draw(
        int width,
        int height,
        SpriteBatch spriteBatch,
        Microsoft.Xna.Framework.Rectangle bounds);
}
