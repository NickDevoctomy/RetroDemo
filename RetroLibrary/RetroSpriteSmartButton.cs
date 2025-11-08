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
    private bool isToggleButton;

    [ObservableProperty]
    private bool isToggled;

    public RetroSpriteSmartButton(
        string name,
        string text,
        Point position,
        Point size,
        bool isToggleButton,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        Color? tint = null,
        SmartButtonTexture2D? upSmartButtonTexture = null,
        SmartButtonTexture2D? downSmartButtonTexture = null,
        SpriteFont? font = null,
        bool buffered = true)
        : base(
            name,
            position,
            size,
            backgroundColor,
            foregroundColor,
            font,
            buffered,
            false)
    {
        Text = text;
        IsToggleButton = isToggleButton;
        UpSmartButtonTexture = upSmartButtonTexture;
        DownSmartButtonTexture = downSmartButtonTexture;
        Tint = tint ?? Color.White;

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
        propertyNames.Add(nameof(IsToggled));
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        UpSmartButtonTexture?.Dispose();
        UpSmartButtonTexture = null;
        DownSmartButtonTexture?.Dispose();
        DownSmartButtonTexture = null;
    }

    protected override void OnClicked()
    {
        base.OnClicked();
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
        var texture = isPressed ? DownSmartButtonTexture : UpSmartButtonTexture;
        if (texture != null)
        {
            var buttonTexture = texture!.BuildTexture(
                spriteBatch.GraphicsDevice,
                Size.X,
                Size.Y);
            spriteBatch.Draw(
                buttonTexture,
                new Rectangle(location, Size),
                Tint);
        }

        if (Font != null && !string.IsNullOrEmpty(Text))
        {
            var yOffset = isPressed ? 2 : 0;
            Vector2 textSize = Font.MeasureString(Text);
            Vector2 textPosition = new Vector2(
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
