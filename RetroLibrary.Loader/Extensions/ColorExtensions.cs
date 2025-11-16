using System.Reflection;
using Microsoft.Xna.Framework;

namespace RetroLibrary.Loader.Extensions;

public static class ColorExtensions
{
    private static readonly Dictionary<Color, string> NamesByColor = new();
    private static readonly Dictionary<string, Color> ColorsByName = new();
    private static readonly object LockObject = new();

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

    private static void CacheColors()
    {
        lock(LockObject)
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
