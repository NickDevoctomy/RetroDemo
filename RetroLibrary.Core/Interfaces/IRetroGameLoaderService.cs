using Microsoft.Xna.Framework;
using RetroLibrary.Core.Base;

namespace RetroLibrary.Core.Interfaces;

public interface IRetroGameLoaderService
{
    public string Name { get; }

    public Color BackgroundColor { get; }

    public List<RetroSpriteBase> Sprites { get; }

    public bool LoadGame(RetroGameContext gameContext);
}
