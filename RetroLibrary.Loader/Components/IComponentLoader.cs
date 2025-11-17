using System.Xml.Linq;
using RetroLibrary.Core;

namespace RetroLibrary.XmlLoader.Components;

public interface IComponentLoader
{
    public bool IsApplicable(XElement element);

    public (string Id, object Value) LoadComponent(
        RetroGameContext gameContext,
        XElement element);
}
