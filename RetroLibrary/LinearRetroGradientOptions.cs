using Microsoft.Xna.Framework;

namespace RetroLibrary;

public class LinearRetroGradientOptions : IEquatable<LinearRetroGradientOptions>
{
    public Point FromPoint { get; set; }

    public Point ToPoint { get; set; }

    public Color FromColor { get; set; } = Color.White;
    
    public Color ToColor { get; set; } = Color.Black;

    public int GradientStops { get; set; } = 8;

    public bool Equals(LinearRetroGradientOptions? other)
    {
        return other != null &&
               FromPoint.Equals(other.FromPoint) &&
               ToPoint.Equals(other.ToPoint) &&
               FromColor.Equals(other.FromColor) &&
               ToColor.Equals(other.ToColor) &&
               GradientStops == other.GradientStops;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as LinearRetroGradientOptions);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FromPoint, ToPoint, FromColor, ToColor, GradientStops);
    }
}
