using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Core;
using RetroLibrary.Core.Resources;

namespace RetroLibrary.XmlLoader.Resources;

public class SpriteFontResourceLoader : ResourceLoaderBase, IResourceLoader
{
    public bool IsApplicable(XElement element)
    {
        return element.Name == "Font";
    }

    // !!! TODO: Needs to be called during game LoadContent phase !!!
    public (string Id, object Value) LoadResource(
        RetroGameContext gameContext,
        XElement element)
    {
        var id = element.Attribute("id")!.Value;
        SpriteFont? font = null;
        try
        {
            font = gameContext.ContentManager!.Load<SpriteFont>(id);
        }
        catch
        {
            // Ignore exceptions, will handle below
        }

        return (font == null)
            ? throw new Exception($"Failed to load font resource with id '{id}'.")
            : (id, (object)font);
    }
}