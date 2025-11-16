using Microsoft.Xna.Framework.Graphics;

namespace RetroLibrary.Resources;

public interface ITexture2DResourceLoader
{
    public Texture2D FromFile(string path);
}
