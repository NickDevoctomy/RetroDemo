using Microsoft.Xna.Framework.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace RetroLibrary;

// Use with caution as this is slow as fuck

public class MultiSlicePieTexture2D
{
    private MultiSlicePieOptions? _options;
    private Texture2D? _cachedTexture;

    public void Draw(
        MultiSlicePieOptions options,
        SpriteBatch spriteBatch)
    {
        var texture = BuildTexture(
            spriteBatch.GraphicsDevice,
            options);
        spriteBatch.Draw(
            texture,
            Microsoft.Xna.Framework.Vector2.Zero,
            Microsoft.Xna.Framework.Color.White);
    }

    private Texture2D BuildTexture(
        GraphicsDevice graphicsDevice,
        MultiSlicePieOptions options)
    {
        if (_cachedTexture != null &&
           options.Equals(_options))
        {
            return _cachedTexture;
        }

        if (_cachedTexture != null)
        {
            _cachedTexture.Dispose();
            _cachedTexture = null;
        }

        using var image = new Image<Rgba32>(options.Width, options.Height);
        image.Mutate(ctx =>
        {
            var center = new PointF(options.X, options.Y);
            var sliceAngle = 360f / options.NumSlices;
            var builder = new PathBuilder();

            for (int i = 0; i < options.NumSlices; i++)
            {
                var startAngle = options.Angle + (i * sliceAngle);
                var endAngle = startAngle + sliceAngle;

                if (i % 2 == 0)
                {
                    builder.Reset();
                    builder.AddLine(center,
                        new PointF(center.X + options.Radius * MathF.Cos(MathF.PI * startAngle / 180f),
                                   center.Y + options.Radius * MathF.Sin(MathF.PI * startAngle / 180f)));
                    builder.AddArc(center, options.Radius, options.Radius, 0, startAngle, sliceAngle);
                    builder.AddLine(
                        new PointF(center.X + options.Radius * MathF.Cos(MathF.PI * endAngle / 180f),
                                   center.Y + options.Radius * MathF.Sin(MathF.PI * endAngle / 180f)),
                        center);

                    var path = builder.Build();
                    ctx.Fill(options.Color, path);
                }
            }
        });
        using var ms = new MemoryStream();
        image.SaveAsPng(ms);
        ms.Seek(0, SeekOrigin.Begin);
        return Texture2D.FromStream(graphicsDevice, ms);
    }
}