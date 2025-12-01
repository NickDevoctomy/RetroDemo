using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Drawing;

namespace RetroLibrary.Controls;

public partial class RetroSpriteNineSliceButton : RetroSpriteBase
{
    [ObservableProperty]
    private string text = string.Empty;

    [ObservableProperty]
    private NineSliceTexture2D? upTexture;

    [ObservableProperty]
    private NineSliceTexture2D? downTexture;

    [ObservableProperty]
    private Color upTint;

    [ObservableProperty]
    private Color downTint;

    [ObservableProperty]
    private bool isToggleButton;

    [ObservableProperty]
    private bool isToggled;

    [ObservableProperty]
    private RelayCommand? clickCommand;

    [ObservableProperty]
    private SoundEffect? clickSound;

    public RetroSpriteNineSliceButton(
        string name,
        string text,
        Point position,
        Point size,
        bool isToggleButton,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        Color? upTint = null,
        Color? downTint = null,
        NineSliceTexture2D? upTexture = null,
        NineSliceTexture2D? downTexture = null,
        RelayCommand? clickCommand = null,
        SoundEffect? clickSound = null,
        SpriteFont? font = null,
        Rectangle? margins = null,
        Rectangle? padding = null,
        bool isVisible = true)
        : base(
            name,
            position,
            size,
            backgroundColor,
            foregroundColor,
            font,
            margins,
            padding,
            isVisible)
    {
        Text = text;
        IsToggleButton = isToggleButton;
        UpTexture = upTexture;
        DownTexture = downTexture;
        UpTint = upTint ?? Color.White;
        DownTint = downTint ?? Color.White;
        ClickCommand = clickCommand;
        ClickSound = clickSound;
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        UpTexture?.Dispose();
        UpTexture = null;
        DownTexture?.Dispose();
        DownTexture = null;
    }

    protected override void OnClicked()
    {
        base.OnClicked();

        ClickSound?.Play();
        ClickCommand?.Execute(null);

        if (IsToggleButton)
        {
            IsToggled = !IsToggled;
        }
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
        var isPressed = IsToggleButton ? IsToggled : IsPressed;
        var texture = isPressed ? DownTexture : UpTexture;
        if (texture != null)
        {
            var buttonTexture = texture!.BuildTexture(
                spriteBatch.GraphicsDevice,
                Size.X,
                Size.Y);
            spriteBatch.Draw(
                buttonTexture,
                new Rectangle(location, Size),
                isPressed ? DownTint : UpTint);
        }

        if (Font != null && !string.IsNullOrEmpty(Text))
        {
            var yOffset = isPressed ? 2 : 0;
            var textSize = Font.MeasureString(Text);
            var textPosition = new Vector2(
                location.X + ((Size.X - textSize.X) / 2),
                location.Y + yOffset + ((Size.Y - textSize.Y) / 2));

            spriteBatch.DrawString(
                Font,
                Text,
                textPosition,
                ForegroundColor);
        }
    }
}