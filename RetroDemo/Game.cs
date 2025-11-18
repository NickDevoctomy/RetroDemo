using RetroLibrary.Core;
using RetroLibrary.Core.Base;

namespace RetroDemo;

public class Game : RetroGameBase
{
    ////private readonly Random _rnd = new(Environment.TickCount);

    public Game(RetroGameContext retroGameContext)
        : base(retroGameContext)
    {
    }

    ////private void ExitButton_Clicked(object? sender, EventArgs e)
    ////{
    ////    this.Exit();
    ////}

    ////private void TestButton_Clicking(object? sender, EventArgs e)
    ////{
    ////    if (sender is RetroSpriteNineSliceButton button)
    ////    {
    ////        button.Text = "Clicking";
    ////    }
    ////}

    ////private void TestButton_Clicked(object? sender, EventArgs e)
    ////{
    ////    if (sender is RetroSpriteNineSliceButton button)
    ////    {
    ////        button.Text = "Clicked!";
    ////        _testProgressBar!.Value = (float)_rnd.NextDouble();
    ////    }
    ////}
}
