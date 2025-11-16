using Microsoft.Xna.Framework.Graphics;

namespace RetroLibrary.Resources;

public class Texture2DResourceLoader(GraphicsDevice graphicsDevice) : ITexture2DResourceLoader
{
    private readonly Dictionary<string, Texture2D> _cachedTextures = new ();

    public Texture2D FromFile(string path)
    {
        if (_cachedTextures.TryGetValue(path, out var cachedTexture))
        {
            return cachedTexture;
        }

        var texture2D = Texture2D.FromFile(graphicsDevice, path);
        _cachedTextures.Add(path, texture2D);
        return texture2D;
    }
}
