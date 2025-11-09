using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RetroLibrary;

public partial class RetroSpriteLabel : RetroSpriteBase
{
    [ObservableProperty]
    private string text = string.Empty;

    public RetroSpriteLabel(
        string name,
        string text,
        Point position,
        Point size,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
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
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
        // Only centre aligned for now
        if (Font != null && !string.IsNullOrEmpty(Text))
        {
            Vector2 textSize = Font.MeasureString(Text);
            Vector2 textPosition = new Vector2(
                location.X + ((Size.X - textSize.X) / 2),
                location.Y + ((Size.Y - textSize.Y) / 2));

            spriteBatch.DrawString(
                Font,
                Text,
                textPosition,
                ForegroundColor);
        }
    }
}
