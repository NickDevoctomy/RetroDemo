using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RetroLibrary;

public class SmartButtonTexture2D : IDisposable
{
    private int _cachedWidth;
    private int _cachedHeight;
    private Texture2D? _cachedTexture;
    private bool disposedValue;

    public Texture2D BuildTexture(
        GraphicsDevice graphicsDevice,
        Texture2D source,
        int width,
        int height,
        SmartButtonOptions options)
    {
        if(_cachedTexture != null &&
           _cachedWidth == width &&
           _cachedHeight == height)
        {
            return _cachedTexture;
        }

        if (_cachedTexture != null)
        {
            _cachedTexture.Dispose();
            _cachedTexture = null;
        }

        var renderTarget = new RenderTarget2D(graphicsDevice, width, height);
        var spriteBatch = new SpriteBatch(graphicsDevice);

        graphicsDevice.SetRenderTarget(renderTarget);
        graphicsDevice.Clear(Color.Transparent);

        spriteBatch.Begin();

        // 9-Slice drawing

        // Top left corner
        spriteBatch.Draw(
            source,
            new Rectangle(0, 0, options.LeftMargin, options.TopMargin),
            new Rectangle(0, 0, options.LeftMargin, options.TopMargin),
            Color.White);

        // Left edge
        spriteBatch.Draw(
            source,
            new Rectangle(0, options.TopMargin, options.LeftMargin, height - options.TopMargin - options.BottomMargin),
            new Rectangle(0, options.TopMargin, options.LeftMargin, source.Height - options.TopMargin - options.BottomMargin),
            Color.White);

        // Bottom left corner
        spriteBatch.Draw(
            source,
            new Rectangle(0, height - options.BottomMargin, options.LeftMargin, options.BottomMargin),
            new Rectangle(0, source.Height - options.BottomMargin, options.LeftMargin, options.BottomMargin),
            Color.White);

        // Top edge
        spriteBatch.Draw(
            source,
            new Rectangle(options.LeftMargin, 0, width - options.LeftMargin - options.RightMargin, options.TopMargin),
            new Rectangle(options.LeftMargin, 0, source.Width - options.LeftMargin - options.RightMargin, options.TopMargin),
            Color.White);

        // Top right corner
        spriteBatch.Draw(
            source,
            new Rectangle(width - options.RightMargin, 0, options.RightMargin, options.TopMargin),
            new Rectangle(source.Width - options.RightMargin, 0, options.RightMargin, options.TopMargin),
            Color.White);

        // Right edge
        spriteBatch.Draw(
            source,
            new Rectangle(width - options.RightMargin, options.TopMargin, options.RightMargin, height - options.TopMargin - options.BottomMargin),
            new Rectangle(source.Width - options.RightMargin, options.TopMargin, options.RightMargin, source.Height - options.TopMargin - options.BottomMargin),
            Color.White);

        // Bottom right corner
        spriteBatch.Draw(
            source,
            new Rectangle(width - options.RightMargin, height - options.BottomMargin, options.RightMargin, options.BottomMargin),
            new Rectangle(source.Width - options.RightMargin, source.Height - options.BottomMargin, options.RightMargin, options.BottomMargin),
            Color.White);

        // Bottom edge
        spriteBatch.Draw(
            source,
            new Rectangle(options.LeftMargin, height - options.BottomMargin, width - options.LeftMargin - options.RightMargin, options.BottomMargin),
            new Rectangle(options.LeftMargin, source.Height - options.BottomMargin, source.Width - options.LeftMargin - options.RightMargin, options.BottomMargin),
            Color.White);

        // Center
        spriteBatch.Draw(
            source,
            new Rectangle(options.LeftMargin, options.TopMargin, width - options.LeftMargin - options.RightMargin, height - options.TopMargin - options.BottomMargin),
            new Rectangle(options.LeftMargin, options.TopMargin, source.Width - options.LeftMargin - options.RightMargin, source.Height - options.TopMargin - options.BottomMargin),
            Color.White);

        spriteBatch.End();
        graphicsDevice.SetRenderTarget(null);
        spriteBatch.Dispose();

        _cachedWidth = width;
        _cachedHeight = height;
        _cachedTexture = renderTarget;

        return renderTarget;
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

    ~SmartButtonTexture2D()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
