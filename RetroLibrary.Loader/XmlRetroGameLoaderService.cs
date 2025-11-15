using System.Xml.Linq;

namespace RetroLibrary.Loader;

public class XmlRetroGameLoaderService : IRetroGameLoaderService
{
    public List<RetroSpriteBase> Sprites { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public async Task<bool> LoadGameAsync(string filePath, CancellationToken cancellationToken)
    {
        using var stream = File.OpenRead(filePath);
        var document = await XDocument.LoadAsync(
            stream,
            LoadOptions.None,
            cancellationToken);

        // Parse XML document here

        return false;
    }
}
