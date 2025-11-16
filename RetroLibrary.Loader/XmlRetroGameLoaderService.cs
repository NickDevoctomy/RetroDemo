using System.Xml.Linq;
using RetroLibrary.Loader.Components;
using RetroLibrary.Loader.Resources;

namespace RetroLibrary.Loader;

public class XmlRetroGameLoaderService(
    IEnumerable<IResourceLoader> resourceLoaders,
    IEnumerable<IComponentLoader> componentLoaders) : IRetroGameLoaderService
{
    public List<RetroSpriteBase> Sprites { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public async Task<bool> LoadGameAsync(
        RetroGameContext gameContext,
        CancellationToken cancellationToken)
    {
        using var stream = File.OpenRead(gameContext.GameDefinitionFilePath);
        var document = await XDocument.LoadAsync(
            stream,
            LoadOptions.None,
            cancellationToken);

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
                (string id, object value) = await resourceLoader.LoadResourceAsync(
                    gameContext,
                    resourceElement,
                    cancellationToken);

                gameContext.ResourceManager.AddResource(id, value);
            }
        }

        var componentsRoot = document.Root.Element("Components");
        if (componentsRoot != null)
        {
            foreach (var componentElement in componentsRoot.Elements())
            {
                var componentLoader = componentLoaders.Single(loader => loader.IsApplicable(componentElement));
                await componentLoader.LoadComponentAsync(
                    gameContext,
                    componentElement,
                    cancellationToken);
            }
        }

        return false;
    }
}
