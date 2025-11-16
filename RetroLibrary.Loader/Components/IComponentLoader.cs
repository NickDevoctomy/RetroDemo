using System.Xml.Linq;

namespace RetroLibrary.Loader.Components;

public interface IComponentLoader
{
    public bool IsApplicable(XElement element);

    public Task<(string Id, object Value)> LoadComponentAsync(
        RetroGameContext gameContext,
        XElement element,
        CancellationToken cancellationToken);
}
