using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Xna.Framework.Audio;
using RetroLibrary.Controls;
using RetroLibrary.Core;
using RetroLibrary.Core.Base;

namespace RetroDemo;

public partial class GameViewModel(RetroGameBase game) : RetroGameViewModelBase
{
    private Random _rnd = new(Environment.TickCount);
    private RetroSpriteLabel? _fpsLabel;

    [ObservableProperty]
    private int fps;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName == nameof(Fps))
        {
            // Need tp implement binding for label text
            var label = GetFpsLabel();
            if (label != null)
            {
                label.Text = $"FPS: {Fps}";
            }
        }
    }

    private RetroSpriteLabel? GetFpsLabel()
    {
        if (game.RetroGameContext.RetroGameLoaderService.FindSpriteByName("FpsLabel") is RetroSpriteLabel fpsLabel)
        {
            _fpsLabel = fpsLabel;
        }

        return _fpsLabel;
    }

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
        else
        {
        }
    }

    [RelayCommand]
    private void OrangeButton_Clicked()
    {
        // Only works when tab is active at the moment, need to be able to search all children in all tabs
        if (game.RetroGameContext.RetroGameLoaderService.FindSpriteByName("TestProgressBar") is RetroSpriteProgressBar progress)
        {
            var sound = game.RetroGameContext.ResourceManager.GetResource<SoundEffect>("updateSound");
            sound.Play();
            progress.Value = (float)_rnd.NextDouble();
        }
        else
        {
            var sound = game.RetroGameContext.ResourceManager.GetResource<SoundEffect>("buttonClick1Sound");
            sound.Play();
        }
    }
}