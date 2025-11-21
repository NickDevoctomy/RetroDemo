using System.Xml.Linq;

namespace RetroLibrary.Core.Resources;

public interface IResourceLoader
{
    public bool IsApplicable(XElement element);

    public (string Id, object Value) LoadResource(
        RetroGameContext gameContext,
        XElement element);
}