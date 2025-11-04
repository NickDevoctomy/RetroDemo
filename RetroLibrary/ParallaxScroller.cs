using Microsoft.Xna.Framework.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace RetroLibrary;

public class ParallaxScroller(ParallaxScrollerOptions options) : IDisposable
{
    private ParallaxScrollerOptions _options = options;
    private bool _layerTexturesCached = false;

    private Dictionary<ParallaxScrollerLayer, ParallaxScrollerLayerState> _layerOffsets = [];
    private bool disposedValue;

    public void Draw(SpriteBatch spriteBatch)
    {
        var rect = new Microsoft.Xna.Framework.Rectangle(0, 0, _options.ViewportWidth, _options.ViewportHeight);

        if(!_layerTexturesCached)
        {
            CacheLayerTextures(spriteBatch);
            _layerTexturesCached = true;
        }

        var startYOffset = (int?)null;
        var firstLayer = _options.Layers.FirstOrDefault();
        if(firstLayer != null)
        {
            if (!_layerOffsets.TryGetValue(firstLayer, out var state))
            {
                throw new Exception("Layer state not found for first layer.");
            }

            startYOffset = _options.ViewportHeight - state.Texture!.Bounds.Height;
        }

        for (var i = _options.Layers.Count - 1; i >= 0; i--)
        {
            var layer = _options.Layers[i];
            if (!_layerOffsets.TryGetValue(layer, out var state))
            {
                continue;
            }

            if (state.Texture == null)
            {
                state.Texture = Texture2D.FromFile(spriteBatch.GraphicsDevice, layer.TexturePath);

                using var image = Image.Load<Rgba32>(layer.TexturePath);
                image.Mutate(ctx =>
                {
                    ctx.Flip(FlipMode.Horizontal);
                });
                using var ms = new MemoryStream();
                image.SaveAsPng(ms);
                ms.Seek(0, SeekOrigin.Begin);
                state.TextureFlipped = Texture2D.FromStream(spriteBatch.GraphicsDevice, ms);
            }

            var fullSize = state.Texture.Bounds.Size;
            var offsetPixelsX = state.Offset * fullSize.X;

            var copiesRequired = (int)Math.Ceiling(((decimal)rect.Width + fullSize.X) / fullSize.X) + 1;

            var flip = state.FirstFlipped;
            for (var x = 0; x < copiesRequired - 1; x++)
            {
                var image = flip ? state.TextureFlipped : state.Texture;
                var curOffsetX = offsetPixelsX + (x * fullSize.X);

                spriteBatch.Draw(image, new Microsoft.Xna.Framework.Rectangle((int)curOffsetX, startYOffset.GetValueOrDefault() - layer.YOffset, fullSize.X, fullSize.Y), Microsoft.Xna.Framework.Color.White);

                flip = !flip;
            }
        }
    }

    public void Update()
    {
        foreach (var layer in _options.Layers)
        {
            UpdateLayerOffsets(layer);
        }
    }

    private void CacheLayerTextures(SpriteBatch spriteBatch)
    {
        foreach (var layer in _options.Layers)
        {
            if (!_layerOffsets.TryGetValue(layer, out var state))
            {
                continue;
            }

            if (state.Texture == null)
            {
                state.Texture = Texture2D.FromFile(spriteBatch.GraphicsDevice, layer.TexturePath);

                using var image = Image.Load<Rgba32>(layer.TexturePath);
                image.Mutate(ctx =>
                {
                    ctx.Flip(FlipMode.Horizontal);
                });
                using var ms = new MemoryStream();
                image.SaveAsPng(ms);
                ms.Seek(0, SeekOrigin.Begin);
                state.TextureFlipped = Texture2D.FromStream(spriteBatch.GraphicsDevice, ms);
            }
        }
    }

    private void UpdateLayerOffsets(ParallaxScrollerLayer layer)
    {
        if(!_layerOffsets.TryGetValue(layer, out var state))
        {
            state = new ParallaxScrollerLayerState();
            _layerOffsets.Add(layer, state);
            return;
        }

        var percentPerPixel = 1f / _options.ViewportWidth;

        if (state.Offset < -1f)
        {
            state.Offset = state.Offset + 1f;
            state.FirstFlipped = !state.FirstFlipped;
        }

        state.Offset -= layer.ScrollSpeed * percentPerPixel;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            foreach (var layerState in _layerOffsets.Values)
            {
                layerState.Dispose();
            }
            _layerOffsets.Clear();

            disposedValue = true;
        }
    }

    ~ParallaxScroller()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
