using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Core.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace RetroLibrary.Core.Drawing;

public class RadialRetroGradientTexture2D(RadialRetroGradientOptions options) : IRetroTexture2D, IDisposable
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
        var texture = _cachedTexture ?? CreateGradient(
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
}
