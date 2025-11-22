using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Drawing;

namespace RetroLibrary.Controls;

public partial class RetroSpriteProgressBar : RetroSpriteBase
{
    [ObservableProperty]
    private float value;

    [ObservableProperty]
    private NineSliceTexture2D? borderTexture;

    [ObservableProperty]
    private Color? borderTint;

    [ObservableProperty]
    private Color? fromColor;

    [ObservableProperty]
    private Color? toColor;

    private LinearRetroGradientTexture2D? _progressTexture;

    public RetroSpriteProgressBar(
        string name,
        float value,
        Point position,
        Point size,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        NineSliceTexture2D? borderTexture = null,
        Color? borderTint = null,
        Color? fromColor = null,
        Color? toColor = null,
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
        Value = value;
        BorderTexture = borderTexture;
        BorderTint = borderTint;
        FromColor = fromColor;
        ToColor = toColor;

        _progressTexture = new LinearRetroGradientTexture2D(new LinearRetroGradientOptions
        {
            FromColor = FromColor ?? Color.Green,
            ToColor = ToColor ?? Color.Red,
            FromPoint = new Point(0, 0),
            ToPoint = new Point(Size.X, 0)
        });
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
        var locationOffset = new Point(
            location.X + 1,
            location.Y + 1);
        var sizeOffset = new Point(
            Size.X - 2,
            Size.Y - 2);

        int width = (int)(sizeOffset.X * Value);

        _progressTexture?.Draw(
            sizeOffset.X,
            sizeOffset.Y,
            spriteBatch,
            new Rectangle(0, 0, width, sizeOffset.Y),
            new Rectangle(locationOffset.X, locationOffset.Y, width, sizeOffset.Y));

        if (BorderTexture != null)
        {
            var borderLocation = new Point(
                location.X,
                location.Y);
            var borderSize = new Point(
                Size.X,
                Size.Y);
            var borderTexture = BorderTexture.BuildTexture(
                spriteBatch.GraphicsDevice,
                borderSize.X,
                borderSize.Y);
            spriteBatch.Draw(
                borderTexture,
                new Rectangle(borderLocation, borderSize),
                BorderTint ?? Color.White);
        }
    }
}