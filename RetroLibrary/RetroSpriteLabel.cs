using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Enums;
using RetroLibrary.Extensions;

namespace RetroLibrary;

public partial class RetroSpriteLabel : RetroSpriteBase
{
    [ObservableProperty]
    private string text = string.Empty;

    [ObservableProperty]
    private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Middle;

    [ObservableProperty]
    private VerticalAlignment verticalAlignment = Enums.VerticalAlignment.Middle;

    public RetroSpriteLabel(
        string name,
        string text,
        Point position,
        Point size,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        HorizontalAlignment horizontalAlignment = HorizontalAlignment.Middle,
        VerticalAlignment veritcalAlignment = Enums.VerticalAlignment.Middle,
        SpriteFont? font = null,
        bool buffered = true,
        bool updateWatchedProperties = true)
        : base(
            name,
            position,
            size,
            backgroundColor,
            foregroundColor,
            font,
            buffered,
            updateWatchedProperties)
    {
        Text = text;
        HorizontalAlignment = horizontalAlignment;
        VerticalAlignment = veritcalAlignment;
    }

    public override void SetWatchedProperties(List<string> propertyNames)
    {
        base.SetWatchedProperties(propertyNames);
        propertyNames.Add(nameof(Text));
        propertyNames.Add(nameof(HorizontalAlignment));
        propertyNames.Add(nameof(VerticalAlignment));
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
        // Only centre aligned for now
        if (Font != null && !string.IsNullOrEmpty(Text))
        {
            Vector2 textSize = Font.MeasureString(Text);

            Vector2 textPosition = textSize.Align(
                new Rectangle(location, Size),
                HorizontalAlignment,
                (VerticalAlignment)VerticalAlignment);

            spriteBatch.DrawString(
                Font,
                Text,
                textPosition,
                ForegroundColor);
        }
    }
}
