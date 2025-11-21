using System.Xml.Linq;
using Microsoft.Xna.Framework.Audio;
using RetroLibrary.Core;
using RetroLibrary.Core.Resources;

namespace RetroLibrary.XmlLoader.Resources;

public class SoundResourceLoader : ResourceLoaderBase, IResourceLoader
{
    public bool IsApplicable(XElement element)
    {
        return element.Name == "Sound";
    }

    public (string Id, object Value) LoadResource(
        RetroGameContext gameContext,
        XElement element)
    {
        var id = element.Attribute("id")!.Value;
        var path = element.Attribute("path")!.Value;
        var soundEffect = SoundEffect.FromFile(path);
        return (id, soundEffect);
    }
}