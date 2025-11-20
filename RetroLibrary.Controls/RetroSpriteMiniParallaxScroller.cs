using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Resources;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace RetroLibrary.Controls;

public partial class RetroSpriteMiniParallaxScroller : RetroSpriteBase
{
    [ObservableProperty]
    private List<MiniParallaxScrollerMiniParallaxScrollerLayer> layers;

    private readonly Dictionary<MiniParallaxScrollerMiniParallaxScrollerLayer, MiniParallaxScrollerLayerState> _layerOffsets = new ();
    private bool _layerTexturesCached = false;
    private Texture2DResourceLoader textureLoader = new (); // We need to inject this or get it from the context

    public RetroSpriteMiniParallaxScroller(
        string name,
        Microsoft.Xna.Framework.Point position,
        Microsoft.Xna.Framework.Point size,
        Microsoft.Xna.Framework.Color? backgroundColor = null,
        Microsoft.Xna.Framework.Color? foregroundColor = null,
        List<MiniParallaxScrollerMiniParallaxScrollerLayer>? layers = null,
        SpriteFont? font = null,
        bool buffered = false,
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
        Layers = layers ?? new List<MiniParallaxScrollerMiniParallaxScrollerLayer>();
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);

        foreach (var layerState in _layerOffsets.Values)
        {
            layerState.Dispose();
        }

        _layerOffsets.Clear();
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Microsoft.Xna.Framework.Point location)
    {
        var rect = new Microsoft.Xna.Framework.Rectangle(location + Position, Size);

        if (!_layerTexturesCached)
        {
            CacheLayerTextures(spriteBatch);
            _layerTexturesCached = true;
        }

        var startYOffset = (int?)null;
        var firstLayer = Layers.FirstOrDefault();
        if (firstLayer != null)
        {
            if (!_layerOffsets.TryGetValue(firstLayer, out var state))
            {
                throw new Exception("Layer state not found for first layer.");
            }

            startYOffset = Size.Y - state.Texture!.Bounds.Height;
        }

        for (var i = Layers.Count - 1; i >= 0; i--)
        {
            var layer = Layers[i];
            if (!_layerOffsets.TryGetValue(layer, out var state))
            {
                continue;
            }

            var fullSize = state.Texture!.Bounds.Size;
            var offsetPixelsX = state.Offset * fullSize.X;

            var copiesRequired = (int)Math.Ceiling(((decimal)rect.Width + fullSize.X) / fullSize.X) + 1;

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

    protected override void OnUpdate(
        MouseState mouseState, MouseState
        previousMouseState)
    {
        foreach (var layer in Layers)
        {
            UpdateLayerOffsets(layer);
        }
    }

    private static void AlternateFlipTile(
        SpriteBatch spriteBatch,
        int? startYOffset,
        MiniParallaxScrollerMiniParallaxScrollerLayer layer,
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
        foreach (var layer in Layers)
        {
            if (!_layerOffsets.TryGetValue(layer, out var state))
            {
                continue;
            }

            if (state.Texture == null)
            {
                state.Texture = textureLoader.FromFile(
                    spriteBatch.GraphicsDevice,
                    layer.TexturePath);

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

    private void UpdateLayerOffsets(MiniParallaxScrollerMiniParallaxScrollerLayer layer)
    {
        if (!_layerOffsets.TryGetValue(layer, out var state))
        {
            state = new MiniParallaxScrollerLayerState();
            _layerOffsets.Add(layer, state);
            return;
        }

        var percentPerPixel = 1f / Size.X;

        if (state.Offset < -1f)
        {
            state.Offset += 1f;
            state.FirstFlipped = !state.FirstFlipped;
        }

        state.Offset -= layer.ScrollSpeed * percentPerPixel;
    }
}
