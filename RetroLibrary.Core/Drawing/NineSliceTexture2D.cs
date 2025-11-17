using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RetroLibrary.Core.Drawing;

public class NineSliceTexture2D(
    Texture2D sourceTexture,
    NineSliceTextureOptions options) : IDisposable
{
    private Dictionary<(int width, int height), Texture2D> _textureCache = new ();
    private Texture2D? _cachedTexture;
    private bool disposedValue;

    ~NineSliceTexture2D()
    {
        Dispose(disposing: false);
    }

    public Texture2D BuildTexture(
        GraphicsDevice graphicsDevice,
        int width,
        int height)
    {
        var cachedTexture = CheckCache(
            width,
            height);
        if (cachedTexture != null)
        {
            return cachedTexture;
        }

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
        graphicsDevice.Clear(Color.Transparent);

        spriteBatch.Begin();

        // 9-Slice drawing

        // Top left corner
        spriteBatch.Draw(
            sourceTexture,
            new Rectangle(0, 0, options.LeftMargin, options.TopMargin),
            new Rectangle(0, 0, options.LeftMargin, options.TopMargin),
            Color.White);

        // Left edge
        spriteBatch.Draw(
            sourceTexture,
            new Rectangle(0, options.TopMargin, options.LeftMargin, height - options.TopMargin - options.BottomMargin),
            new Rectangle(0, options.TopMargin, options.LeftMargin, sourceTexture.Height - options.TopMargin - options.BottomMargin),
            Color.White);

        // Bottom left corner
        spriteBatch.Draw(
            sourceTexture,
            new Rectangle(0, height - options.BottomMargin, options.LeftMargin, options.BottomMargin),
            new Rectangle(0, sourceTexture.Height - options.BottomMargin, options.LeftMargin, options.BottomMargin),
            Color.White);

        // Top edge
        spriteBatch.Draw(
            sourceTexture,
            new Rectangle(options.LeftMargin, 0, width - options.LeftMargin - options.RightMargin, options.TopMargin),
            new Rectangle(options.LeftMargin, 0, sourceTexture.Width - options.LeftMargin - options.RightMargin, options.TopMargin),
            Color.White);

        // Top right corner
        spriteBatch.Draw(
            sourceTexture,
            new Rectangle(width - options.RightMargin, 0, options.RightMargin, options.TopMargin),
            new Rectangle(sourceTexture.Width - options.RightMargin, 0, options.RightMargin, options.TopMargin),
            Color.White);

        // Right edge
        spriteBatch.Draw(
            sourceTexture,
            new Rectangle(width - options.RightMargin, options.TopMargin, options.RightMargin, height - options.TopMargin - options.BottomMargin),
            new Rectangle(sourceTexture.Width - options.RightMargin, options.TopMargin, options.RightMargin, sourceTexture.Height - options.TopMargin - options.BottomMargin),
            Color.White);

        // Bottom right corner
        spriteBatch.Draw(
            sourceTexture,
            new Rectangle(width - options.RightMargin, height - options.BottomMargin, options.RightMargin, options.BottomMargin),
            new Rectangle(sourceTexture.Width - options.RightMargin, sourceTexture.Height - options.BottomMargin, options.RightMargin, options.BottomMargin),
            Color.White);

        // Bottom edge
        spriteBatch.Draw(
            sourceTexture,
            new Rectangle(options.LeftMargin, height - options.BottomMargin, width - options.LeftMargin - options.RightMargin, options.BottomMargin),
            new Rectangle(options.LeftMargin, sourceTexture.Height - options.BottomMargin, sourceTexture.Width - options.LeftMargin - options.RightMargin, options.BottomMargin),
            Color.White);

        // Center
        spriteBatch.Draw(
            sourceTexture,
            new Rectangle(options.LeftMargin, options.TopMargin, width - options.LeftMargin - options.RightMargin, height - options.TopMargin - options.BottomMargin),
            new Rectangle(options.LeftMargin, options.TopMargin, sourceTexture.Width - options.LeftMargin - options.RightMargin, sourceTexture.Height - options.TopMargin - options.BottomMargin),
            Color.White);

        spriteBatch.End();
        graphicsDevice.SetRenderTargets(originalRenderTargets);

        CacheTexture(
            width,
            height,
            renderTarget);

        return renderTarget;
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

    private void CacheTexture(
        int width,
        int height,
        Texture2D texture)
    {
        switch (options.CachingMode)
        {
            case Enums.CachingMode.Single:
                {
                    _cachedTexture?.Dispose();
                    _cachedTexture = texture;
                    break;
                }

            case Enums.CachingMode.BySize:
                {
                    if (_textureCache.TryGetValue((width, height), out var existingTexture))
                    {
                        existingTexture.Dispose();
                    }

                    _textureCache[(width, height)] = texture;

                    System.Diagnostics.Debug.WriteLine($"Cached textures count: {_textureCache.Count}");

                    break;
                }
        }
    }

    private Texture2D? CheckCache(
        int width,
        int height)
    {
        switch (options.CachingMode)
        {
            case Enums.CachingMode.Single:
                {
                    if (_cachedTexture != null &&
                       _cachedTexture.Width == width &&
                       _cachedTexture.Height == height)
                    {
                        return _cachedTexture;
                    }

                    _cachedTexture?.Dispose();
                    _cachedTexture = null;

                    return null;
                }

            case Enums.CachingMode.BySize:
                {
                    if (_textureCache.TryGetValue((width, height), out var cachedTexture))
                    {
                        return cachedTexture;
                    }

                    return null;
                }
        }

        return null;
    }
}
