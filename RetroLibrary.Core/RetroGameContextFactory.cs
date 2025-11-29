using RetroLibrary.Core.Interfaces;
using RetroLibrary.Core.Resources;

namespace RetroLibrary.Core;

public class RetroGameContextFactory(
    IRetroGamePreLoaderService retroGamePreLoaderService,
    IRetroGameLoaderService retroGameLoaderService,
    ITexture2DResourceLoader texture2DResourceLoader) : IRetroGameContextFactory
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
            retroGamePreLoaderService,
            retroGameLoaderService,
            texture2DResourceLoader);
    }
}