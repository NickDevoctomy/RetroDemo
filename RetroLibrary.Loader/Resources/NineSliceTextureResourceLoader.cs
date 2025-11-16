using System.Xml.Linq;
using RetroLibrary.Resources;

namespace RetroLibrary.Loader.Resources;

public class NineSliceTextureResourceLoader(ITexture2DResourceLoader texture2DResourceLoader) : IResourceLoader
{
    public bool IsApplicable(XElement element)
    {
        return element.Name == "NineSliceTexture";
    }

    public Task<(string Id, object Value)> LoadResourceAsync(
        RetroGameContext gameContext,
        XElement element,
        CancellationToken cancellationToken)
    {
        var id = element.Attribute("id")!.Value;
        var nineSliceTexture2D = new NineSliceTexture2D(
                texture2DResourceLoader.FromFile(element.Attribute("path")!.Value),
                new NineSliceTextureOptions
                {
                    TopMargin = int.Parse(element.Attribute("top")!.Value),
                    LeftMargin = int.Parse(element.Attribute("left")!.Value),
                    BottomMargin = int.Parse(element.Attribute("bottom")!.Value),
                    RightMargin = int.Parse(element.Attribute("right")!.Value)
                });

        return nineSliceTexture2D == null
            ? throw new Exception($"Failed to load font resource with id '{id}'.")
            : (Task<(string Id, object Value)>)Task.FromResult((id, (object)nineSliceTexture2D));
    }
}
