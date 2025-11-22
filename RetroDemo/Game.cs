using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RetroLibrary.Core;
using RetroLibrary.Core.Base;

namespace RetroDemo;

public class Game(RetroGameContext retroGameContext)
    : RetroGameBase(retroGameContext)
{

    protected override void OnUpdate(
        GameTime gameTime,
        MouseState currentState,
        MouseState previousState)
    {
        base.OnUpdate(
            gameTime,
            currentState,
            previousState);

        if (RetroGameContext.RetroGameLoaderService.ViewModel is GameViewModel gameViewModel)
        {
            gameViewModel.FpsLabelText = $"FPS : {Fps}";
        }
    }
}
