using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Resources;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace RetroLibrary;

public class MiniParallaxScroller(
    Texture2DResourceLoader textureLoader,
    MiniParallaxScrollerOptions options) : IDisposable
{
    private readonly Dictionary<MiniParallaxScrollerLayer, MiniParallaxScrollerLayerState> _layerOffsets = new ();
    private bool _layerTexturesCached = false;
    private bool disposedValue;

    ~MiniParallaxScroller()
    {
        Dispose(disposing: false);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var rect = new Microsoft.Xna.Framework.Rectangle(0, 0, options.ViewportWidth, options.ViewportHeight);

        if (!_layerTexturesCached)
        {
            CacheLayerTextures(spriteBatch);
            _layerTexturesCached = true;
        }

        var startYOffset = (int?)null;
        var firstLayer = options.Layers.FirstOrDefault();
        if (firstLayer != null)
        {
            if (!_layerOffsets.TryGetValue(firstLayer, out var state))
            {
                throw new Exception("Layer state not found for first layer.");
            }

            startYOffset = options.ViewportHeight - state.Texture!.Bounds.Height;
        }

        for (var i = options.Layers.Count - 1; i >= 0; i--)
        {
            var layer = options.Layers[i];
            if (!_layerOffsets.TryGetValue(layer, out var state))
            {
                continue;
            }

            var fullSize = state.Texture!.Bounds.Size;
            var offsetPixelsX = state.Offset * fullSize.X;

            var copiesRequired = (int)Math.Ceiling(((decimal)rect.Width + fullSize.X) / fullSize.X) + 1;

            // We really need to cache the result of this, based on copiesRequired and FirstFlipped state
            var flip = state.FirstFlipped;
            AlternateFlipTile(
                spriteBatch,
                startYOffset,
                layer,
                state,
                fullSize,
                offsetPixelsX,
                copiesRequired,
                flip);
        }
    }

    public void Update()
    {
        foreach (var layer in options.Layers)
        {
            UpdateLayerOffsets(layer);
        }
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

            foreach (var layerState in _layerOffsets.Values)
            {
                layerState.Dispose();
            }

            _layerOffsets.Clear();
            disposedValue = true;
        }
    }

    private static void AlternateFlipTile(
        SpriteBatch spriteBatch,
        int? startYOffset,
        MiniParallaxScrollerLayer layer,
        MiniParallaxScrollerLayerState state,
        Microsoft.Xna.Framework.Point fullSize,
        float offsetPixelsX,
        int copiesRequired,
        bool flip)
    {
        for (var x = 0; x < copiesRequired - 1; x++)
        {
            var image = flip ? state.TextureFlipped : state.Texture;
            var curOffsetX = offsetPixelsX + (x * fullSize.X);

            spriteBatch.Draw(
                image,
                new Microsoft.Xna.Framework.Rectangle((int)curOffsetX, startYOffset.GetValueOrDefault() - layer.YOffset, fullSize.X, fullSize.Y),
                Microsoft.Xna.Framework.Color.White);

            flip = !flip;
        }
    }

    private void CacheLayerTextures(SpriteBatch spriteBatch)
    {
        foreach (var layer in options.Layers)
        {
            if (!_layerOffsets.TryGetValue(layer, out var state))
            {
                continue;
            }

            if (state.Texture == null)
            {
                state.Texture = textureLoader.FromFile(layer.TexturePath);

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

    private void UpdateLayerOffsets(MiniParallaxScrollerLayer layer)
    {
        if (!_layerOffsets.TryGetValue(layer, out var state))
        {
            state = new MiniParallaxScrollerLayerState();
            _layerOffsets.Add(layer, state);
            return;
        }

        var percentPerPixel = 1f / options.ViewportWidth;

        if (state.Offset < -1f)
        {
            state.Offset += 1f;
            state.FirstFlipped = !state.FirstFlipped;
        }

        state.Offset -= layer.ScrollSpeed * percentPerPixel;
    }
}
