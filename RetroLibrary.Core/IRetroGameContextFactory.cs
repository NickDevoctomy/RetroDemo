using RetroLibrary.Core.Components;

namespace RetroLibrary.Core;

public interface IRetroGameContextFactory
{
    public RetroGameContext CreateRetroGameContext(
        int width,
        int height,
        bool isFullScreen,
        string gameDefinitionFilePath,
        IEnumerable<IComponentLoader> componentLoaders);
}
