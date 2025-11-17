using RetroLibrary.Core.Base;

namespace RetroLibrary.Core.Interfaces;

public interface IRetroGameLoaderService
{
    public List<RetroSpriteBase> Sprites { get; set; }

    public bool LoadGame(RetroGameContext gameContext);
}
