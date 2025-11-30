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
        string gameDefinitionFilePath)
    {
        return new RetroGameContext(
            args,
            gameDefinitionFilePath,
            retroGamePreLoaderService,
            retroGameLoaderService,
            texture2DResourceLoader);
    }
}