using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Drawing;
using RetroLibrary.Core.Enums;
using RetroLibrary.Core.Extensions;

namespace RetroLibrary.Controls;

public partial class RetroSpriteCheckBox : RetroSpriteBase
{
    [ObservableProperty]
    private string text = string.Empty;

    [ObservableProperty]
    private NineSliceTexture2D? boxTexture;

    [ObservableProperty]
    private bool isChecked;

    public RetroSpriteCheckBox(
        string name,
        string text,
        Point position,
        Point size,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        NineSliceTexture2D? boxTexture = null,
        bool isChecked = false,
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
        BoxTexture = boxTexture;
        IsChecked = isChecked;
    }

    protected override void OnClicked()
    {
        base.OnClicked();
        IsChecked = !IsChecked;
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
        // location already includes this sprite's Position from base Draw.
        if (BoxTexture != null)
        {
            var boxTexture = BoxTexture.BuildTexture(
                spriteBatch.GraphicsDevice,
                24,
                24);

            spriteBatch.Draw(
                boxTexture,
                new Rectangle(
                    location.X,
                    location.Y,
                    boxTexture.Width,
                    boxTexture.Height),
                Color.White);

            if (IsChecked && Font != null)
            {
                spriteBatch.DrawString(
                    Font,
                    "X",
                    new Vector2(location.X + 6, location.Y + 6),
                    ForegroundColor);
            }
        }

        if (Font != null && !string.IsNullOrEmpty(Text))
        {
            var textRect = new Rectangle(
                location.X + 30,
                location.Y,
                Size.X - 30,
                Size.Y);

            var textSize = Font.MeasureString(Text);

            var aligned = textSize.Align(
                textRect,
                HorizontalAlignment.Left,
                VerticalAlignment.Middle);

            aligned.Y += 2; // slight nudge to center vertically better

            spriteBatch.DrawString(
                Font,
                Text,
                aligned,
                ForegroundColor);
        }
    }
}