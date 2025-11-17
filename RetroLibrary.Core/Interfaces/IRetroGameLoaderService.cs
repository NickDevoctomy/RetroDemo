using RetroLibrary.Core.Base;

namespace RetroLibrary.Core.Interfaces;

public interface IRetroGameLoaderService
{
    public List<RetroSpriteBase> Sprites { get; set; }

    public Task<bool> LoadGameAsync(
        RetroGameContext gameContext,
        CancellationToken cancellationToken);
}
