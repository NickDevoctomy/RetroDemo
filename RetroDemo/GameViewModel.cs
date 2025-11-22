using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RetroLibrary.Controls;
using RetroLibrary.Core.Base;

namespace RetroDemo;

public partial class GameViewModel(RetroGameBase game) : RetroGameViewModelBase
{
    private Random _rnd = new(Environment.TickCount);

    [ObservableProperty]
    private string fpsLabelText;

    [RelayCommand]
    private void ExitButton_Clicked()
    {
        game.Exit();
    }

    [RelayCommand]
    private void AppleTabButton_Clicked()
    {
        var scroller = game.RetroGameContext.RetroGameLoaderService.Sprites
            .SingleOrDefault(x => x.Name == "ParalaxScroller");
        if (scroller != null)
        {
            scroller.IsVisible = !scroller.IsVisible;
        }
    }

    [RelayCommand]
    private void OrangeButton_Clicked()
    {
        // Only works when tab is active at the moment, need to be able to search all children in all tabs
        if (game.RetroGameContext.RetroGameLoaderService.FindSpriteByName("TestProgressBar") is RetroSpriteProgressBar progress)
        {
            progress.Value = (float)_rnd.NextDouble();
        }
    }
}