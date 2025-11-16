using System.Xml.Linq;
using RetroLibrary.Loader.Resources;

namespace RetroLibrary.Loader;

public class XmlRetroGameLoaderService(IEnumerable<IResourceLoader> resourceLoaders) : IRetroGameLoaderService
{
    public Dictionary<string, object> Resources = [];
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

        if(document == null ||
            document.Root == null)
        {
            return false;
        }

        var resourcesRoot = document.Root.Element("Resources");
        if(resourcesRoot != null)
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

        return false;
    }
}
