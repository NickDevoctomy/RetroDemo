using System.Xml.Linq;
using RetroLibrary.Core;
using RetroLibrary.Core.Configuration;
using RetroLibrary.Core.Interfaces;

namespace RetroLibrary.XmlLoader;

public class XmlRetroGamePreLoaderService : IRetroGamePreLoaderService
{
    public RetroGameConfiguration PreLoad(RetroGameContext retroGameContext)
    {
        using var stream = File.OpenRead(retroGameContext.GameDefinitionFilePath);
        var document = XDocument.Load(
            stream,
            LoadOptions.None);

        if (document == null ||
            document.Root == null)
        {
            return new RetroGameConfiguration();
        }

        var width = document.Root.Attribute("width") != null ? int.Parse(document.Root.Attribute("width")!.Value) : 800;
        var height = document.Root.Attribute("height") != null ? int.Parse(document.Root.Attribute("height")!.Value) : 600;
        var isFullScreen = document.Root.Attribute("isFullScreen") != null && bool.Parse(document.Root.Attribute("isFullScreen")!.Value);

        return new RetroGameConfiguration
        {
            Width = width,
            Height = height,
            IsFullScreen = isFullScreen
        };
    }
}