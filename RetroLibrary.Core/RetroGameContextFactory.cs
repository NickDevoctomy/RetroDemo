using RetroLibrary.Core.Interfaces;

namespace RetroLibrary.Core;

public class RetroGameContextFactory(IRetroGameLoaderService retroGameLoaderService) : IRetroGameContextFactory
{
    public RetroGameContext CreateRetroGameContext(
        int width,
        int height,
        bool isFullScreen,
        string gameDefinitionFilePath)
    {
        return new RetroGameContext(
            width,
            height,
            isFullScreen,
            gameDefinitionFilePath,
            retroGameLoaderService);
    }
}
