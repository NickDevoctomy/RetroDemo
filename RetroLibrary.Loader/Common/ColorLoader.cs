using Microsoft.Xna.Framework;
using RetroLibrary.Loader.Extensions;

namespace RetroLibrary.Loader.Common;

public class ColorLoader : IColorLoader
{
    public Color ColorFromName(
        string name,
        Color defaultColor)
    {
        return name.ToColorFromName() ?? defaultColor;
    }
}
