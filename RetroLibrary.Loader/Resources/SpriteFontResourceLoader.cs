using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace RetroLibrary.Loader.Resources;

public class SpriteFontResourceLoader : IResourceLoader
{
    public bool IsApplicable(XElement element)
    {
        return element.Name == "Font";
    }

    public Task<(string Id, object Value)> LoadResourceAsync(
        RetroGameContext gameContext,
        XElement element,
        CancellationToken cancellationToken)
    {
        var id = element.Attribute("id")!.Value;
        SpriteFont? font = null;
        try
        {
            font = gameContext.ContentManager.Load<SpriteFont>(id);
        }
        catch
        {
            // Ignore exceptions, will handle below
        }

        return (font == null)
            ? throw new Exception($"Failed to load font resource with id '{id}'.")
            : Task.FromResult((id, (object)font));
    }
}
