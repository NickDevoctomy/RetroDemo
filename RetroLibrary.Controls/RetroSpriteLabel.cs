using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Core.Attributes;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Binding;
using RetroLibrary.Core.Enums;
using RetroLibrary.Core.Extensions;

namespace RetroLibrary.Controls;

public partial class RetroSpriteLabel : RetroSpriteBase
{
    [RetroSpriteBindableProperty]
    [ObservableProperty]
    private BindingValue<string>? text;

    [ObservableProperty]
    private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Middle;

    [ObservableProperty]
    private VerticalAlignment verticalAlignment = VerticalAlignment.Middle;

    public RetroSpriteLabel(
        string name,
        Point position,
        Point size,
        BindingValue<string>? text = null,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        HorizontalAlignment horizontalAlignment = HorizontalAlignment.Middle,
        VerticalAlignment verticalAlignment = VerticalAlignment.Middle,
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
        Text = text ?? new BindingValue<string>(string.Empty);
        HorizontalAlignment = horizontalAlignment;
        VerticalAlignment = verticalAlignment;
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Rectangle bounds)
    {
        if (Font != null && !string.IsNullOrEmpty(Text?.Value))
        {
            Vector2 textSize = Font.MeasureString(Text.Value);

            Vector2 textPosition = textSize.Align(
                bounds,
                HorizontalAlignment,
                VerticalAlignment);

            spriteBatch.DrawString(
                Font,
                Text.Value,
                textPosition,
                ForegroundColor);
        }
    }
}