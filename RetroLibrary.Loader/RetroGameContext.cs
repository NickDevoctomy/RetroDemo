using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using RetroLibrary.Loader.Resources;

namespace RetroLibrary.Loader;

public class RetroGameContext(
    GraphicsDeviceManager graphicsDeviceManager,
    ContentManager contentManager,
    IResourceManager resourceManager)
{
    public GraphicsDeviceManager GraphicsDeviceManager { get; } = graphicsDeviceManager;
    public ContentManager ContentManager { get; } = contentManager;
    public string GameDefinitionFilePath { get; set; } = string.Empty;
    public IResourceManager ResourceManager { get; } = resourceManager;
}
