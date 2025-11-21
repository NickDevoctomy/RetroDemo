using Microsoft.Xna.Framework.Graphics;

namespace RetroLibrary.Controls;

public class MiniParallaxScrollerLayerState : IDisposable
{
    private bool disposedValue;

    ~MiniParallaxScrollerLayerState()
    {
        Dispose(disposing: false);
    }

    public Texture2D? Texture { get; set; }

    public Texture2D? TextureFlipped { get; set; }

    // New composite texture that contains the horizontally tiled (alternating flipped) pattern
    public Texture2D? CompositeTexture { get; set; }

    public float Offset { get; set; }

    public bool FirstFlipped { get; set; }

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
                Texture?.Dispose();
                Texture = null;

                TextureFlipped?.Dispose();
                TextureFlipped = null;

                CompositeTexture?.Dispose();
                CompositeTexture = null;
            }

            disposedValue = true;
        }
    }
}