using System.Reflection;
using Microsoft.Xna.Framework;
using SixLabors.ImageSharp.Drawing.Processing;

namespace RetroLibrary.Core.Extensions;

public static class ColorExtensions
{
    private static readonly Dictionary<Color, string> NamesByColor = new ();
    private static readonly Dictionary<string, Color> ColorsByName = new ();
    private static readonly object LockObject = new ();

    public static string? ToName(this Color color)
    {
        CacheColors();
        return NamesByColor.TryGetValue(color, out var name)
            ? name
            : null;
    }

    public static Color? ToColorFromName(this string name)
    {
        CacheColors();
        return ColorsByName.TryGetValue(name, out var color)
            ? color
            : null;
    }

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

    private static void CacheColors()
    {
        lock (LockObject)
        {
            if (NamesByColor.Count > 0)
            {
                return;
            }

            var properties = typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(Color))
                {
                    var color = (Color?)property.GetValue(null, null);
                    if (color == null)
                    {
                        continue;
                    }

                    if (!NamesByColor.ContainsKey(color.GetValueOrDefault()))
                    {
                        NamesByColor.Add(color.GetValueOrDefault(), property.Name);
                    }

                    if (!ColorsByName.ContainsKey(property.Name))
                    {
                        ColorsByName.Add(property.Name, color.GetValueOrDefault());
                    }
                }
            }
        }
    }
}
