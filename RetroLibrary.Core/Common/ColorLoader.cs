using Microsoft.Xna.Framework;
using RetroLibrary.Core.Extensions;

namespace RetroLibrary.Core.Common;

public class ColorLoader : IColorLoader
{
    public Color? ColorFromName(
        string name,
        Color? defaultColor)
    {
        return name.ToColorFromName() ?? defaultColor;
    }
}