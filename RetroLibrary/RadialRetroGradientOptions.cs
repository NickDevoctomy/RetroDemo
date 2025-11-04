using SixLabors.ImageSharp;

namespace RetroLibrary;

public class RadialRetroGradientOptions : IEquatable<RadialRetroGradientOptions>
{
    public PointF CentrePoint { get; set; }

    public float Radius { get; set; }

    public Microsoft.Xna.Framework.Color FromColor { get; set; } = Microsoft.Xna.Framework.Color.White;

    public Microsoft.Xna.Framework.Color ToColor { get; set; } = Microsoft.Xna.Framework.Color.Black;

    public int GradientStops { get; set; } = 8;

    public bool Equals(RadialRetroGradientOptions? other)
    {
        return other != null &&
               CentrePoint.Equals(other.CentrePoint) &&
               FromColor.Equals(other.FromColor) &&
               ToColor.Equals(other.ToColor) &&
               GradientStops == other.GradientStops;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as RadialRetroGradientOptions);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(CentrePoint, FromColor, ToColor, GradientStops);
    }
}
