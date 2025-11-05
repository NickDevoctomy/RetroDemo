using SixLabors.ImageSharp;

namespace RetroLibrary;

public class MultiSlicePieOptions : IEquatable<MultiSlicePieOptions>
{
    public int X { get; set; }
    public int Y { get; set; }
    public float Radius { get; set; }
    public float Angle { get; set; }
    public int NumSlices { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Color Color { get; set; }

    public bool Equals(MultiSlicePieOptions? other)
    {
        return other != null &&
               X == other.X &&
               Y == other.Y &&
               Radius == other.Radius &&
               Angle == other.Angle &&
               NumSlices == other.NumSlices &&
               Width == other.Width &&
               Height == other.Height &&
               Color.Equals(other.Color);
    }

    public bool OnlyAngleDiffers(MultiSlicePieOptions? other)
    {
        return other != null &&
               X == other.X &&
               Y == other.Y &&
               Radius == other.Radius &&
               NumSlices == other.NumSlices &&
               Width == other.Width &&
               Height == other.Height &&
               Color.Equals(other.Color) &&
               Angle != other.Angle;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as LinearRetroGradientOptions);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Radius, Angle, NumSlices, Width, Height, Color);
    }
}
