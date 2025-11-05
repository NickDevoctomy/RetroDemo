using Microsoft.Xna.Framework.Graphics;

namespace RetroLibrary;

public class MiniParallaxScrollerLayerState : IDisposable
{
    private bool disposedValue;

    public Texture2D? Texture { get; set; }
    public Texture2D? TextureFlipped { get; set; }
    public float Offset { get; set; }
    public bool FirstFlipped { get; set; }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            Texture?.Dispose();
            Texture = null;

            TextureFlipped?.Dispose();
            TextureFlipped = null;

            disposedValue = true;
        }
    }

    ~MiniParallaxScrollerLayerState()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
