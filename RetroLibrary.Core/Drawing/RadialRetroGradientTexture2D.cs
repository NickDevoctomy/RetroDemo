using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Core.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace RetroLibrary.Core.Drawing;

public class RadialRetroGradientTexture2D(RadialRetroGradientOptions options) : IDisposable
{
    private int _cachedWidth;
    private int _cachedHeight;
    private Texture2D? _cachedTexture;
    private bool disposedValue;

    ~RadialRetroGradientTexture2D()
    {
        Dispose(disposing: false);
    }

    public void Draw(
        int width,
        int height,
        SpriteBatch spriteBatch,
        Microsoft.Xna.Framework.Rectangle bounds)
    {
        var texture = BuildTexture(
            spriteBatch.GraphicsDevice,
            width,
            height,
            options);
        spriteBatch.Draw(
            texture,
            bounds,
            Microsoft.Xna.Framework.Color.White);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            _cachedTexture?.Dispose();
            _cachedTexture = null;

            disposedValue = true;
        }
    }

    private static Texture2D CreateGradient(
        GraphicsDevice graphicsDevice,
        int width,
        int height,
        RadialRetroGradientOptions options)
    {
        using var image = new Image<Rgba32>(width, height);

        image.Mutate(ctx =>
        {
            var gradientColors = options.FromColor.GetGradientColors(
                options.ToColor,
                options.GradientStops);

            var gradientStops = gradientColors.GetColorStops();

            var gradientBrush = new RadialGradientBrush(
                options.CentrePoint,
                options.Radius,
                GradientRepetitionMode.None,
                [.. gradientStops]);

            ctx.Fill(gradientBrush);
        });

        using var ms = new MemoryStream();
        image.SaveAsPng(ms);
        ms.Seek(0, SeekOrigin.Begin);

        return Texture2D.FromStream(graphicsDevice, ms);
    }

    private Texture2D BuildTexture(
        GraphicsDevice graphicsDevice,
        int width,
        int height,
        RadialRetroGradientOptions options)
    {
        if (_cachedTexture != null &&
           _cachedWidth == width &&
           _cachedHeight == height)
        {
            return _cachedTexture;
        }

        _cachedTexture?.Dispose();
        _cachedTexture = null;

        var renderTarget = new RenderTarget2D(
            graphicsDevice,
            width,
            height,
            false,
            SurfaceFormat.Color,
            DepthFormat.None,
            0,
            RenderTargetUsage.PlatformContents);
        var spriteBatch = new SpriteBatch(graphicsDevice);

        graphicsDevice.SetRenderTarget(renderTarget);
        graphicsDevice.Clear(Microsoft.Xna.Framework.Color.Transparent);

        spriteBatch.Begin();

        var baseGradient = CreateGradient(
            graphicsDevice,
            width,
            height,
            options);

        spriteBatch.Draw(
            baseGradient,
            new Microsoft.Xna.Framework.Rectangle(0, 0, width, height),
            new Microsoft.Xna.Framework.Rectangle(0, 0, width, height),
            Microsoft.Xna.Framework.Color.White);

        spriteBatch.End();
        graphicsDevice.SetRenderTarget(null);
        spriteBatch.Dispose();

        _cachedWidth = width;
        _cachedHeight = height;
        _cachedTexture = renderTarget;

        return _cachedTexture;
    }
}
