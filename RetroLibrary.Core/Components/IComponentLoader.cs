using System.Xml.Linq;

namespace RetroLibrary.Core.Components;

public interface IComponentLoader
{
    public bool IsApplicable(XElement element);

    public (string Id, object Value) LoadComponent(
        RetroGameContext gameContext,
        XElement element);
}
