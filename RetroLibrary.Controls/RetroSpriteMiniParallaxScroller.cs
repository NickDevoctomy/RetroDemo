using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroLibrary.Core.Base;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace RetroLibrary.Controls;

public partial class RetroSpriteMiniParallaxScroller : RetroSpriteBase
{
    private readonly Dictionary<MiniParallaxScrollerMiniParallaxScrollerLayer, MiniParallaxScrollerLayerState> _layerOffsets = [];

    [ObservableProperty]
    private List<MiniParallaxScrollerMiniParallaxScrollerLayer> layers;

    private bool _layerTexturesCached;

    public RetroSpriteMiniParallaxScroller(
        string name,
        Microsoft.Xna.Framework.Point position,
        Microsoft.Xna.Framework.Point size,
        Microsoft.Xna.Framework.Color? backgroundColor = null,
        Microsoft.Xna.Framework.Color? foregroundColor = null,
        List<MiniParallaxScrollerMiniParallaxScrollerLayer>? layers = null,
        SpriteFont? font = null,
        Microsoft.Xna.Framework.Rectangle? margins = null,
        Microsoft.Xna.Framework.Rectangle? padding = null,
        bool isVisible = true)
        : base(
            name,
            position,
            size,
            backgroundColor,
            foregroundColor,
            font,
            margins,
            padding,
            isVisible)
    {
        Layers = layers ?? [];
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
        if (!_layerTexturesCached)
        {
            CacheLayerTextures(spriteBatch);
            _layerTexturesCached = true;
        }

        var startYOffset = (int?)null;
        var firstLayer = Layers.FirstOrDefault();
        if (firstLayer != null && _layerOffsets.TryGetValue(firstLayer, out var firstState) && firstState.Texture != null)
        {
            startYOffset = Size.Y - firstState.Texture.Bounds.Height;
        }

        for (var i = Layers.Count - 1; i >= 0; i--)
        {
            var layer = Layers[i];
            if (!_layerOffsets.TryGetValue(layer, out var state) || state.Texture == null || state.CompositeTexture == null)
            {
                continue;
            }

            var tileWidth = state.Texture.Width;
            var offsetPixelsX = state.Offset * tileWidth;

            ////System.Diagnostics.Debug.WriteLine($"Drawing paralax layer {layer.Name}, with a width of {state.CompositeTexture.Width}.");

            spriteBatch.Draw(
                state.CompositeTexture,
                new Microsoft.Xna.Framework.Rectangle(
                    (int)offsetPixelsX,
                    startYOffset.GetValueOrDefault() - layer.YOffset,
                    state.CompositeTexture.Width,
                    state.CompositeTexture.Height),
                Microsoft.Xna.Framework.Color.White);
        }
    }

    protected override void OnUpdate(
        MouseState mouseState,
        MouseState previousMouseState)
    {
        foreach (var layer in Layers)
        {
            UpdateLayerOffsets(layer);
        }
    }

    private void CacheLayerTextures(SpriteBatch spriteBatch)
    {
        foreach (var layer in Layers)
        {
            if (!_layerOffsets.TryGetValue(layer, out var state))
            {
                state = new MiniParallaxScrollerLayerState();
                _layerOffsets.Add(layer, state);
            }

            if (state.Texture == null)
            {
                state.Texture = RetroGameContext!.Texture2DResourceLoader.FromFile(spriteBatch.GraphicsDevice, layer.TexturePath);

                using var image = Image.Load<Rgba32>(layer.TexturePath);
                using var flipped = image.Clone(ctx => ctx.Flip(FlipMode.Horizontal));
                using var msFlipped = new MemoryStream();
                flipped.SaveAsPng(msFlipped);
                msFlipped.Seek(0, SeekOrigin.Begin);
                state.TextureFlipped = Texture2D.FromStream(spriteBatch.GraphicsDevice, msFlipped);
            }

            if (state.CompositeTexture == null && state.Texture != null && state.TextureFlipped != null)
            {
                BuildCompositeTexture(
                    spriteBatch.GraphicsDevice,
                    layer,
                    state);
            }
        }
    }

    private void BuildCompositeTexture(
        GraphicsDevice graphicsDevice,
        MiniParallaxScrollerMiniParallaxScrollerLayer layer,
        MiniParallaxScrollerLayerState state)
    {
        state.CompositeTexture?.Dispose();
        state.CompositeTexture = null;

        using var sourceImage = Image.Load<Rgba32>(layer.TexturePath);
        using var flippedImage = sourceImage.Clone(ctx => ctx.Flip(FlipMode.Horizontal));

        var tileWidth = sourceImage.Width;
        var tileHeight = sourceImage.Height;

        var baseTilesNeeded = (int)Math.Ceiling((Size.X + tileWidth) / (float)tileWidth) + 1;
        var tilesNeeded = baseTilesNeeded;
        var compositeWidth = tilesNeeded * tileWidth;

        using var composite = new Image<Rgba32>(compositeWidth, tileHeight);

        var flip = state.FirstFlipped;
        for (var i = 0; i < tilesNeeded; i++)
        {
            var src = flip ? flippedImage : sourceImage;
            composite.Mutate(ctx => ctx.DrawImage(src, new Point(i * tileWidth, 0), 1f));
            flip = !flip;
        }

        using var ms = new MemoryStream();
        composite.SaveAsPng(ms);
        ms.Seek(0, SeekOrigin.Begin);
        state.CompositeTexture = Texture2D.FromStream(graphicsDevice, ms);
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
        state.Offset -= layer.ScrollSpeed * percentPerPixel;
        if (state.Offset < -2f) // wrap after two cycles
        {
            state.Offset += 2f;
        }
    }
}