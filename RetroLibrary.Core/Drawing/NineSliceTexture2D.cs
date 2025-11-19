using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RetroLibrary.Core.Drawing;

public class NineSliceTexture2D(
    Texture2D sourceTexture,
    NineSliceTextureOptions options,
    IBlitterService blitterService) : IDisposable
{
    private readonly IBlitterService _blitter = blitterService;
    private readonly Texture2D _sourceTexture = sourceTexture;
    private readonly NineSliceTextureOptions _options = options;

    private readonly Dictionary<(int width, int height), Texture2D> _textureCache = new ();
    private Texture2D? _cachedTexture;
    private Color[] _sourceData = Array.Empty<Color>();
    private bool _setSourceData = false;
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
        var cachedTexture = CheckCache(width, height);
        if (cachedTexture != null)
        {
            return cachedTexture;
        }

        if (width <= 0 || height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "Width/Height must be > 0.");
        }

        var left = _options.LeftMargin;
        var right = _options.RightMargin;
        var top = _options.TopMargin;
        var bottom = _options.BottomMargin;

        if (left + right > width || top + bottom > height)
        {
            throw new ArgumentException("Nine-slice margins exceed target dimensions.");
        }

        EnsureSourceDataLoaded();

        var srcW = _sourceTexture.Width;
        var srcH = _sourceTexture.Height;
        var destData = new Color[width * height];
        var srcInnerW = srcW - left - right;
        var srcInnerH = srcH - top - bottom;
        var dstInnerW = width - left - right;
        var dstInnerH = height - top - bottom;

        var src = _sourceData!;

        // Precompute rects/points
        var srcTopLeft = new Rectangle(0, 0, left, top);
        var srcTopRight = new Rectangle(srcW - right, 0, right, top);
        var srcBottomLeft = new Rectangle(0, srcH - bottom, left, bottom);
        var srcBottomRight = new Rectangle(srcW - right, srcH - bottom, right, bottom);

        var srcTopEdge = new Rectangle(left, 0, srcInnerW, top);
        var srcBottomEdge = new Rectangle(left, srcH - bottom, srcInnerW, bottom);
        var srcLeftEdge = new Rectangle(0, top, left, srcInnerH);
        var srcRightEdge = new Rectangle(srcW - right, top, right, srcInnerH);
        var srcCenter = new Rectangle(left, top, srcInnerW, srcInnerH);

        var dstTopLeft = new Point(0, 0);
        var dstTopRight = new Point(width - right, 0);
        var dstBottomLeft = new Point(0, height - bottom);
        var dstBottomRight = new Point(width - right, height - bottom);

        var dstTopEdge = new Rectangle(left, 0, dstInnerW, top);
        var dstBottomEdge = new Rectangle(left, height - bottom, dstInnerW, bottom);
        var dstLeftEdge = new Rectangle(0, top, left, dstInnerH);
        var dstRightEdge = new Rectangle(width - right, top, right, dstInnerH);
        var dstCenter = new Rectangle(left, top, dstInnerW, dstInnerH);

        // Corners (exact copy)
        _blitter.BlitExact(src, srcW, srcTopLeft, destData, width, dstTopLeft);
        _blitter.BlitExact(src, srcW, srcTopRight, destData, width, dstTopRight);
        _blitter.BlitExact(src, srcW, srcBottomLeft, destData, width, dstBottomLeft);
        _blitter.BlitExact(src, srcW, srcBottomRight, destData, width, dstBottomRight);

        // Edges (scaled in one axis)
        _blitter.BlitScaledNearest(src, srcW, srcTopEdge, destData, width, dstTopEdge); // Top edge
        _blitter.BlitScaledNearest(src, srcW, srcBottomEdge, destData, width, dstBottomEdge); // Bottom edge
        _blitter.BlitScaledNearest(src, srcW, srcLeftEdge, destData, width, dstLeftEdge); // Left edge
        _blitter.BlitScaledNearest(src, srcW, srcRightEdge, destData, width, dstRightEdge); // Right edge

        // Center (scaled both axes)
        _blitter.BlitScaledNearest(src, srcW, srcCenter, destData, width, dstCenter);

        var result = new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);
        result.SetData(destData);

        CacheTexture(width, height, result);
        return result;
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
                foreach (var kv in _textureCache)
                {
                    kv.Value.Dispose();
                }

                _textureCache.Clear();
            }

            _cachedTexture?.Dispose();
            _cachedTexture = null;
            _sourceData = Array.Empty<Color>();

            disposedValue = true;
        }
    }

    private void CacheTexture(
        int width,
        int height,
        Texture2D texture)
    {
        switch (_options.CachingMode)
        {
            case Enums.CachingMode.Single:
                {
                    _cachedTexture?.Dispose();
                    _cachedTexture = texture;
                    break;
                }

            case Enums.CachingMode.BySize:
                {
                    if (_textureCache.TryGetValue((width, height), out var existing))
                    {
                        existing.Dispose();
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
        switch (_options.CachingMode)
        {
            case Enums.CachingMode.Single:
                if (_cachedTexture != null &&
                    _cachedTexture.Width == width &&
                    _cachedTexture.Height == height)
                {
                    return _cachedTexture;
                }

                _cachedTexture?.Dispose();
                _cachedTexture = null;
                return null;

            case Enums.CachingMode.BySize:
                if (_textureCache.TryGetValue((width, height), out var cached))
                {
                    return cached;
                }

                return null;
        }

        return null;
    }

    private void EnsureSourceDataLoaded()
    {
        if (!_setSourceData)
        {
            _sourceData = new Color[_sourceTexture.Width * _sourceTexture.Height];
            _sourceTexture.GetData(_sourceData);
            _setSourceData = true;
        }
    }
}