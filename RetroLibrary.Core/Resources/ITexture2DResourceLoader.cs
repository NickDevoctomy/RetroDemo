using Microsoft.Xna.Framework.Graphics;

namespace RetroLibrary.Core.Resources;

public interface ITexture2DResourceLoader
{
    public Texture2D FromFile(
        GraphicsDevice graphicsDevice,
        string path);
}
