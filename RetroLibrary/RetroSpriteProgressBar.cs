using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RetroLibrary;

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

    private LinearRetroGradientTexture2D? _progressTexture = new LinearRetroGradientTexture2D();
    private Texture2D? _cachedGradientTexture;

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
        Value = value;
        BorderTexture = borderTexture;
        BorderTint = borderTint;
        FromColor = fromColor;
        ToColor = toColor;
    }

    public override void SetWatchedProperties(List<string> propertyNames)
    {
        base.SetWatchedProperties(propertyNames);
        propertyNames.Add(nameof(Value));
        propertyNames.Add(nameof(FromColor));
        propertyNames.Add(nameof(ToColor));
        propertyNames.Add(nameof(BorderTexture));
        propertyNames.Add(nameof(BorderTint));
    }

    protected override void OnRedraw(SpriteBatch spriteBatch, Point location)
    {
        var locationOffset = new Point(
            location.X + 1,
            location.Y + 1);
        var sizeOffset = new Point(
            Size.X - 2,
            Size.Y - 2);
        var gradientProgressTexture = BuildTexture(
            spriteBatch.GraphicsDevice,
            sizeOffset);

        int width = (int)(sizeOffset.X * Value);

        spriteBatch.Draw(
            gradientProgressTexture,
            new Rectangle(locationOffset.X, locationOffset.Y, width, sizeOffset.Y),
            new Rectangle(0, 0, width, sizeOffset.Y),
            Color.White);

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

    private Texture2D BuildTexture(
        GraphicsDevice graphicsDevice,
        Point size)
    {
        if (_cachedGradientTexture != null)
        {
            return _cachedGradientTexture;
        }

        _cachedGradientTexture?.Dispose();
        _cachedGradientTexture = null;

        var renderTarget = new RenderTarget2D(
            graphicsDevice,
            size.X,
            size.Y,
            false,
            SurfaceFormat.Color,
            DepthFormat.None,
            0,
            RenderTargetUsage.PlatformContents);
        using var spriteBatch = new SpriteBatch(graphicsDevice);
        var originalRenderTargets = graphicsDevice.GetRenderTargets();
        graphicsDevice.SetRenderTarget(renderTarget);
        graphicsDevice.Clear(Color.Transparent);
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

        _progressTexture?.Draw(
            size.X,
            size.Y,
            new LinearRetroGradientOptions
            {
                FromColor = FromColor ?? Color.Green,
                ToColor = ToColor ?? Color.Red,
                GradientStops = 8,
                FromPoint = new Point(0, 0),
                ToPoint = new Point(size.X, 0)
            },
            spriteBatch,
            new Rectangle(Point.Zero, size));

        spriteBatch.End();
        graphicsDevice.SetRenderTargets(originalRenderTargets);

        _cachedGradientTexture = renderTarget;
        return _cachedGradientTexture;
    }
}
