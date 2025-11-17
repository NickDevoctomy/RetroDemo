using System.Xml.Linq;
using RetroLibrary.Core;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Interfaces;
using RetroLibrary.Core.Resources;
using RetroLibrary.XmlLoader.Components;

namespace RetroLibrary.XmlLoader;

public class XmlRetroGameLoaderService(
    IEnumerable<IResourceLoader> resourceLoaders,
    IEnumerable<IComponentLoader> componentLoaders) : IRetroGameLoaderService
{
    public List<RetroSpriteBase> Sprites { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
                var componentLoader = componentLoaders.Single(loader => loader.IsApplicable(componentElement));
                (string id, object value) = componentLoader.LoadComponent(
                    gameContext,
                    componentElement);
            }
        }

        return true;
    }
}
