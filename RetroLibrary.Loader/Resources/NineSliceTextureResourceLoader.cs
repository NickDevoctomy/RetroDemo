using System.Xml.Linq;
using RetroLibrary.Core;
using RetroLibrary.Core.Drawing;
using RetroLibrary.Core.Resources;

namespace RetroLibrary.XmlLoader.Resources;

public class NineSliceTextureResourceLoader(
    ITexture2DResourceLoader texture2DResourceLoader,
    IBlitterService blitterService)
    : ResourceLoaderBase, IResourceLoader
{
    public bool IsApplicable(XElement element)
    {
        return element.Name == "NineSliceTexture";
    }

    public (string Id, object Value) LoadResource(
        RetroGameContext gameContext,
        XElement element)
    {
        var id = element.Attribute("id")!.Value;
        var nineSliceTexture2D = new NineSliceTexture2D(
                texture2DResourceLoader.FromFile(
                    gameContext.GraphicsDeviceManager!.GraphicsDevice,
                    element.Attribute("path")!.Value),
                new NineSliceTextureOptions
                {
                    TopMargin = int.Parse(element.Attribute("top")!.Value),
                    LeftMargin = int.Parse(element.Attribute("left")!.Value),
                    BottomMargin = int.Parse(element.Attribute("bottom")!.Value),
                    RightMargin = int.Parse(element.Attribute("right")!.Value)
                },
                blitterService);

        return nineSliceTexture2D == null
            ? throw new Exception($"Failed to load font resource with id '{id}'.")
            : (id, (object)nineSliceTexture2D);
    }
}
