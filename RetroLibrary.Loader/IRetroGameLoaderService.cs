namespace RetroLibrary.Loader;

internal interface IRetroGameLoaderService
{
    public List<RetroSpriteBase> Sprites { get; set; }

    public Task<bool> LoadGameAsync(
        string filePath,
        CancellationToken cancellationToken);
}
