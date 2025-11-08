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
    private Color tint = Color.White;

    [ObservableProperty]
    private SpriteFont? font;

    public override void SetWatchedProperties(List<string> propertyNames)
    {
        base.SetWatchedProperties(propertyNames);
        propertyNames.Add(nameof(Text));
        propertyNames.Add(nameof(UpSmartButtonTexture));
        propertyNames.Add(nameof(DownSmartButtonTexture));
        propertyNames.Add(nameof(Tint));
        propertyNames.Add(nameof(Font));
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
