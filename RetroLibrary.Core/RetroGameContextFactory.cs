using RetroLibrary.Core.Interfaces;
using RetroLibrary.Core.Resources;

namespace RetroLibrary.Core;

public class RetroGameContextFactory(
    IRetroGamePreLoaderService retroGamePreLoaderService,
    IRetroGameLoaderService retroGameLoaderService,
    ITexture2DResourceLoader texture2DResourceLoader) : IRetroGameContextFactory
{
    public RetroGameContext CreateRetroGameContext(
        string[] args,
        int width,
        int height,
        bool isFullScreen,
        string gameDefinitionFilePath)
    {
        // parse args here so things can be overridden from command line if needed in future
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