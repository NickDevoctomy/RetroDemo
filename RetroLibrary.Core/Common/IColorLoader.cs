using Microsoft.Xna.Framework;

namespace RetroLibrary.Core.Common;

public interface IColorLoader
{
    public Color? ColorFromName(
        string name,
        Color? defaultColor);
}
