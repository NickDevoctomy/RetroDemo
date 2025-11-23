using RetroLibrary.Core.Base;
using RetroLibrary.Core.Binding;
using RetroLibrary.Core.Components;
using RetroLibrary.Core.Interfaces;
using RetroLibrary.Core.Resources;

namespace RetroLibrary.Core;

public class RetroGameContextFactory(
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
            retroGameLoaderService,
            texture2DResourceLoader);
    }
}