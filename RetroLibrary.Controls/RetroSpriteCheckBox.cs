using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Drawing;

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
        bool isVisible = true,
        bool buffered = true,
        bool updateWatchedProperties = true)
        : base(
            name,
            position,
            size,
            backgroundColor,
            foregroundColor,
            font,
            isVisible,
            buffered,
            updateWatchedProperties)
    {
        Text = text;
        BoxTexture = boxTexture;
        IsChecked = isChecked;
    }

    public override void SetWatchedProperties(List<string> propertyNames)
    {
        base.SetWatchedProperties(propertyNames);
        propertyNames.Add(nameof(Text));
        propertyNames.Add(nameof(BoxTexture));
        propertyNames.Add(nameof(IsHovered));
        propertyNames.Add(nameof(IsChecked));
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

            if (IsChecked)
            {
                spriteBatch.DrawString(
                    Font,
                    "X",
                    new Vector2(6, 6),
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
            var textPosition = new Vector2(
                textRect.X,
                textRect.Y + ((textRect.Height - textSize.Y) / 2));

            spriteBatch.DrawString(
                Font,
                Text,
                textPosition,
                ForegroundColor);
        }
    }
}