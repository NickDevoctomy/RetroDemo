using Microsoft.Xna.Framework;
using SixLabors.ImageSharp.Drawing.Processing;

namespace RetroLibrary.Extensions;

public static class ColorExtensions
{
    public static List<ColorStop> GetColorStops(this List<Color> colors)
    {
        var colorStops = new List<ColorStop>();

        for (int i = 0; i < colors.Count; i++)
        {
            var curColor = SixLabors.ImageSharp.Color.FromRgba(
                colors[i].R,
                colors[i].G,
                colors[i].B,
                colors[i].A);
            float bandStart = (float)i / colors.Count;
            float bandEnd = (float)(i + 1) / colors.Count;
            colorStops.Add(new ColorStop(bandStart, curColor));
            if (i < colors.Count - 1)
            {
                colorStops.Add(new ColorStop(bandEnd, curColor));
            }
        }

        return colorStops;
    }

    public static List<Color> GetGradientColors(
        this Color from,
        Color to,
        int gradientStops)
    {
        var colors = new List<Color>
        {
            from
        };
        for (int i = 1; i < (gradientStops - 1); i++)
        {
            double colorPosition = (double)i / (gradientStops - 1);
            colors.Add(InterpolateColor(from, to, colorPosition));
        }

        colors.Add(to);

        return colors;
    }

    private static Color InterpolateColor(
        Color from,
        Color to,
        double position)
    {
        position = Math.Max(0, Math.Min(1, position));

        byte r = (byte)(from.R + ((to.R - from.R) * position));
        byte g = (byte)(from.G + ((to.G - from.G) * position));
        byte b = (byte)(from.B + ((to.B - from.B) * position));
        byte a = (byte)(from.A + ((to.A - from.A) * position));

        return new Color(r, g, b, a);
    }
}
