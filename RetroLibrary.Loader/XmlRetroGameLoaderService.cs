using System.Xml.Linq;
using Microsoft.Xna.Framework;
using RetroLibrary.Core;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Components;
using RetroLibrary.Core.Interfaces;
using RetroLibrary.Core.Resources;

namespace RetroLibrary.XmlLoader;

public class XmlRetroGameLoaderService(
    IColorLoader colorLoader,
    IEnumerable<IResourceLoader> resourceLoaders) : IRetroGameLoaderService
{
    public string Name { get; private set; } = "Retro Game";

    public Color BackgroundColor { get; private set; } = Color.Transparent;

    public List<RetroSpriteBase> Sprites { get; private set; } = new ();

    public bool LoadGame(RetroGameContext gameContext)
    {
        using var stream = File.OpenRead(gameContext.GameDefinitionFilePath);
        var document = XDocument.Load(
            stream,
            LoadOptions.None);

        if (document == null ||
            document.Root == null)
        {
            return false;
        }

        Name = document.Root.Element("Name")?.Value ?? "Retro Game";
        var bgColor = document.Root.Attribute("backgroundColor")!.Value;
        BackgroundColor = colorLoader.ColorFromName(
            bgColor,
            Color.Transparent).GetValueOrDefault();

        var resourcesRoot = document.Root.Element("Resources");
        if (resourcesRoot != null)
        {
            foreach (var resourceElement in resourcesRoot.Elements())
            {
                var resourceLoader = resourceLoaders.Single(loader => loader.IsApplicable(resourceElement));
                (string id, object value) = resourceLoader.LoadResource(
                    gameContext,
                    resourceElement);

                gameContext.ResourceManager.AddResource(id, value);
            }
        }

        var componentsRoot = document.Root.Element("Components");
        if (componentsRoot != null)
        {
            foreach (var componentElement in componentsRoot.Elements())
            {
                var componentLoader = gameContext.ComponentLoaders.Single(loader => loader.IsApplicable(componentElement));
                (string id, object value) = componentLoader.LoadComponent(
                    gameContext,
                    componentElement);

                if (value is RetroSpriteBase sprite)
                {
                    Sprites.Add(sprite);
                }
            }
        }

        return true;
    }
}
