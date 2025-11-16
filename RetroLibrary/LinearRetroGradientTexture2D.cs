using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace RetroLibrary;

public class LinearRetroGradientTexture2D : IDisposable
{
    private int _cachedWidth;
    private int _cachedHeight;
    private LinearRetroGradientOptions? _options;
    private Texture2D? _cachedTexture;
    private bool disposedValue;

    ~LinearRetroGradientTexture2D()
    {
        Dispose(disposing: false);
    }

    public void Draw(
        int width,
        int height,
        LinearRetroGradientOptions options,
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
        LinearRetroGradientOptions options)
    {
        using var image = new Image<Rgba32>(width, height);

        image.Mutate(ctx =>
        {
            var gradientColors = options.FromColor.GetGradientColors(
                options.ToColor,
                options.GradientStops);

            var gradientStops = gradientColors.GetColorStops();

            var fromPoint = options.FromPoint == Microsoft.Xna.Framework.Point.Zero && options.ToPoint == Microsoft.Xna.Framework.Point.Zero
                ? new PointF(0, 0)
                : new PointF(options.FromPoint.X, options.FromPoint.Y);

            var toPoint = options.FromPoint == Microsoft.Xna.Framework.Point.Zero && options.ToPoint == Microsoft.Xna.Framework.Point.Zero
                ? new PointF(width, 0)
                : new PointF(options.ToPoint.X, options.ToPoint.Y);

            var gradientBrush = new LinearGradientBrush(
                fromPoint,
                toPoint,
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
        LinearRetroGradientOptions options)
    {
        if (_cachedTexture != null &&
           _cachedWidth == width &&
           _cachedHeight == height &&
           options.Equals(_options))
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
        using var spriteBatch = new SpriteBatch(graphicsDevice);
        var originalRenderTargets = graphicsDevice.GetRenderTargets();
        graphicsDevice.SetRenderTarget(renderTarget);
        graphicsDevice.Clear(Microsoft.Xna.Framework.Color.Transparent);
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

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
        graphicsDevice.SetRenderTargets(originalRenderTargets);

        _cachedWidth = width;
        _cachedHeight = height;
        _options = options;
        _cachedTexture = renderTarget;

        return _cachedTexture;
    }
}
