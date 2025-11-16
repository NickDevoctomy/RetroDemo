using System.Xml.Linq;

namespace RetroLibrary.Loader.Resources;

public interface IResourceLoader
{
    public bool IsApplicable(XElement element);

    public Task<(string Id, object Value)> LoadResourceAsync(
        RetroGameContext gameContext,
        XElement element,
        CancellationToken cancellationToken);
}
