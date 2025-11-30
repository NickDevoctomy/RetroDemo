using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Interfaces;
using RetroLibrary.Core.Resources;

namespace RetroLibrary.Core;

public class RetroGameContext(
    string[] args,
    string gameDefinitionFilePath,
    IRetroGamePreLoaderService retroGamePreLoaderService,
    IRetroGameLoaderService retroGameLoaderService,
    ITexture2DResourceLoader texture2DResourceLoader)
{
    public string[] Args { get; } = args;

    public string GameDefinitionFilePath { get; } = gameDefinitionFilePath;

    public RetroGameBase? Game { get; private set; }

    public GraphicsDeviceManager? GraphicsDeviceManager { get; private set; }

    public ContentManager? ContentManager { get; private set; }

    public IRetroGamePreLoaderService RetroGamePreLoaderService { get; } = retroGamePreLoaderService;

    public IRetroGameLoaderService RetroGameLoaderService { get; } = retroGameLoaderService;

    public IResourceManager ResourceManager { get; } = new ResourceManager();

    public ITexture2DResourceLoader Texture2DResourceLoader { get; } = texture2DResourceLoader;

    public void Initialse(RetroGameBase retroGameBase)
    {
        var configutation = RetroGamePreLoaderService.PreLoad(Args, this);

        System.Diagnostics.Debug.WriteLine($"Initialising Graphics Device {configutation.Width} x {configutation.Height}, FullScreen = {configutation.IsFullScreen}.");
        GraphicsDeviceManager = new GraphicsDeviceManager(retroGameBase)
        {
            PreferredBackBufferWidth = configutation.Width,
            PreferredBackBufferHeight = configutation.Height,
            IsFullScreen = configutation.IsFullScreen
        };
        GraphicsDeviceManager.ApplyChanges();
        Game = retroGameBase;
        ContentManager = retroGameBase.Content;
        ContentManager.RootDirectory = "Content";
    }

    public void LoadGameDefinition()
    {
        var success = RetroGameLoaderService.LoadGame(this);
    }
}