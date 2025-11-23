using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Binding;
using RetroLibrary.Core.Components;
using RetroLibrary.Core.Interfaces;
using RetroLibrary.Core.Resources;

namespace RetroLibrary.Core;

public class RetroGameContext(
    int width,
    int height,
    bool isFullScreen,
    string gameDefinitionFilePath,
    IRetroGameLoaderService retroGameLoaderService,
    ITexture2DResourceLoader texture2DResourceLoader)
{
    public int Width { get; } = width;

    public int Height { get; } = height;

    public bool IsFullScreen { get; } = isFullScreen;

    public string GameDefinitionFilePath { get; } = gameDefinitionFilePath;

    public RetroGameBase? Game { get; private set; }

    public GraphicsDeviceManager? GraphicsDeviceManager { get; private set; }

    public ContentManager? ContentManager { get; private set; }

    public IRetroGameLoaderService RetroGameLoaderService { get; } = retroGameLoaderService;

    public IResourceManager ResourceManager { get; } = new ResourceManager();

    public ITexture2DResourceLoader Texture2DResourceLoader { get; } = texture2DResourceLoader;

    public void Initialse(RetroGameBase retroGameBase)
    {
        GraphicsDeviceManager = new GraphicsDeviceManager(retroGameBase)
        {
            PreferredBackBufferWidth = Width,
            PreferredBackBufferHeight = Height,
            IsFullScreen = IsFullScreen
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