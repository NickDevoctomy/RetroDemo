using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Interfaces;
using RetroLibrary.Core.Resources;

namespace RetroLibrary.Core;

public class RetroGameContext(
    int width,
    int height,
    bool isFullScreen,
    string gameDefinitionFilePath,
    IRetroGameLoaderService retroGameLoaderService)
{
    public int Width { get; set; } = width;

    public int Height { get; set; } = height;

    public bool IsFullScreen { get; set; } = isFullScreen;

    public string GameDefinitionFilePath { get; set; } = gameDefinitionFilePath;

    public GraphicsDeviceManager? GraphicsDeviceManager { get; private set; }

    public ContentManager? ContentManager { get; private set; }

    public IRetroGameLoaderService RetroGameLoaderService { get; } = retroGameLoaderService;

    public IResourceManager ResourceManager { get; } = new ResourceManager();

    public void Initialse(RetroGameBase retroGameBase)
    {
        GraphicsDeviceManager = new GraphicsDeviceManager(retroGameBase)
        {
            PreferredBackBufferWidth = Width,
            PreferredBackBufferHeight = Height,
            IsFullScreen = IsFullScreen
        };
        GraphicsDeviceManager.ApplyChanges();
        ContentManager = retroGameBase.Content;
        ContentManager.RootDirectory = "Content";
    }

    public void LoadGameDefinition()
    {
        var success = RetroGameLoaderService.LoadGame(this);
    }
}
