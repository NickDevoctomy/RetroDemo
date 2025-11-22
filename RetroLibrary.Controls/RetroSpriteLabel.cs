using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Enums;
using RetroLibrary.Core.Extensions;

namespace RetroLibrary.Controls;

public partial class RetroSpriteLabel : RetroSpriteBase
{
    [ObservableProperty]
    private string text = string.Empty;

    [ObservableProperty]
    private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Middle;

    [ObservableProperty]
    private VerticalAlignment verticalAlignment = VerticalAlignment.Middle;

    public RetroSpriteLabel(
        string name,
        string text,
        Point position,
        Point size,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        HorizontalAlignment horizontalAlignment = HorizontalAlignment.Middle,
        VerticalAlignment verticalAlignment = VerticalAlignment.Middle,
        SpriteFont? font = null,
        bool isVisible = true)
        : base(
            name,
            position,
            size,
            backgroundColor,
            foregroundColor,
            font,
            isVisible)
    {
        Text = text;
        HorizontalAlignment = horizontalAlignment;
        VerticalAlignment = verticalAlignment;
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
        if (Font != null && !string.IsNullOrEmpty(Text))
        {
            Vector2 textSize = Font.MeasureString(Text);

            Vector2 textPosition = textSize.Align(
                new Rectangle(location, Size),
                HorizontalAlignment,
                VerticalAlignment);

            spriteBatch.DrawString(
                Font,
                Text,
                textPosition,
                ForegroundColor);
        }
    }
}