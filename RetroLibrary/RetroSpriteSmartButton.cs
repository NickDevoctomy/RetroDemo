using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RetroLibrary;

public partial class RetroSpriteSmartButton : RetroSpriteBase
{
    [ObservableProperty]
    private string text = string.Empty;

    [ObservableProperty]
    private SmartButtonTexture2D? upSmartButtonTexture;

    [ObservableProperty]
    private SmartButtonTexture2D? downSmartButtonTexture;

    [ObservableProperty]
    private Color tint;

    [ObservableProperty]
    private SpriteFont? font;

    public RetroSpriteSmartButton(
        string name,
        string text,
        Point position,
        Point size,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        Color? tint = null,
        SmartButtonTexture2D? upSmartButtonTexture = null,
        SmartButtonTexture2D? downSmartButtonTexture = null,
        SpriteFont? font = null,
        bool buffered = true) :
        base(
            name,
            position,
            size,
            backgroundColor,
            foregroundColor,
            buffered,
            false)
    {
        Text = text;
        UpSmartButtonTexture = upSmartButtonTexture;
        DownSmartButtonTexture = downSmartButtonTexture;
        Tint = tint ?? Color.White;
        Font = font;

        UpdateWatchedProperties();
    }

    public override void SetWatchedProperties(List<string> propertyNames)
    {
        base.SetWatchedProperties(propertyNames);
        propertyNames.Add(nameof(Text));
        propertyNames.Add(nameof(UpSmartButtonTexture));
        propertyNames.Add(nameof(DownSmartButtonTexture));
        propertyNames.Add(nameof(Tint));
        propertyNames.Add(nameof(Font));
        propertyNames.Add(nameof(IsHovered));
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
        var texture = IsPressed ? DownSmartButtonTexture : UpSmartButtonTexture;
        if (texture != null)
        {
            spriteBatch.Draw(
                texture!.BuildTexture(
                    spriteBatch.GraphicsDevice,
                    Size.X,
                    Size.Y),
                new Rectangle(location, Size),
                Tint);
        }

        if (Font != null && !string.IsNullOrEmpty(Text))
        {
            var yOffset = IsPressed ? 2 : 0;
            Vector2 textSize = Font.MeasureString(Text);
            Vector2 textPosition = new Vector2(
                location.X + (Size.X - textSize.X) / 2,
                location.Y + yOffset + (Size.Y - textSize.Y) / 2
            );

            spriteBatch.DrawString(
                Font,
                Text,
                textPosition,
                ForegroundColor);
        }
    }

    public override void Dispose()
    {
        UpSmartButtonTexture?.Dispose();
        UpSmartButtonTexture = null;
        DownSmartButtonTexture?.Dispose();
        DownSmartButtonTexture = null;
    }
}
