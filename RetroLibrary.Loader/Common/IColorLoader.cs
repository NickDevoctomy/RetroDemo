using Microsoft.Xna.Framework;

namespace RetroLibrary.Loader.Common;

public interface IColorLoader
{
    public Color? ColorFromName(
        string name,
        Color? defaultColor);
}
