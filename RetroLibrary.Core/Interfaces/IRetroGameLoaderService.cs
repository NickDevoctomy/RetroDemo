using Microsoft.Xna.Framework;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Binding;

namespace RetroLibrary.Core.Interfaces;

public interface IRetroGameLoaderService
{
    public string Name { get; }

    public Color BackgroundColor { get; }

    public RetroGameViewModelBase? ViewModel { get; }

    public List<RetroSpriteBase> Sprites { get; }

    public IBinder Binder { get; }

    public bool LoadGame(RetroGameContext gameContext);

    public RetroSpriteBase? FindSpriteByName(string name);
}