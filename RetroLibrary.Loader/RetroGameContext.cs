using Microsoft.Xna.Framework.Content;
using RetroLibrary.Loader.Resources;

namespace RetroLibrary.Loader;

public class RetroGameContext(
    ContentManager contentManager,
    IResourceManager resourceManager)
{
    public ContentManager ContentManager { get; } = contentManager;
    public string GameDefinitionFilePath { get; set; } = string.Empty;
    public IResourceManager ResourceManager { get; } = resourceManager;
}
