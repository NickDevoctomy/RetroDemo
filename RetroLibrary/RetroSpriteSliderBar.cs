using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RetroLibrary;

public partial class RetroSpriteSliderBar : RetroSpriteBase
{
    [ObservableProperty]
    private NineSliceTexture2D? _sliderBarTexture;

    [ObservableProperty]
    private Color _sliderBarTint;

    [ObservableProperty]
    private float minimum = 0;

    [ObservableProperty]
    private float maximum = 100;

    [ObservableProperty]
    private float value = 50;

    public RetroSpriteSliderBar(
        string name,
        Point position,
        Point size,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        NineSliceTexture2D? sliderBarTexture = null,
        Color? sliderBarTint = null,
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
        SliderBarTexture = sliderBarTexture;
        SliderBarTint = sliderBarTint ?? Color.White;
    }

    public override void SetWatchedProperties(List<string> propertyNames)
    {
        base.SetWatchedProperties(propertyNames);
        propertyNames.Add(nameof(SliderBarTexture));
        propertyNames.Add(nameof(SliderBarTint));
    }

    protected override void OnRedraw(SpriteBatch spriteBatch, Point location)
    {
        if (SliderBarTexture is not null)
        {
            var sliderBarTexture = SliderBarTexture.BuildTexture(
                spriteBatch.GraphicsDevice,
                Size.X,
                8);

            var rect = new Rectangle(
                location.X,
                location.Y + (Size.Y / 2) - (sliderBarTexture.Height / 2),
                Size.X,
                sliderBarTexture.Height);

            spriteBatch.Draw(
                sliderBarTexture,
                rect,
                SliderBarTint);
        }
    }
}
