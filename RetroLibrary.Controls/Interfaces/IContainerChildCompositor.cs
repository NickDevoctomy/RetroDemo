using Microsoft.Xna.Framework;
using RetroLibrary.Core.Base;

namespace RetroLibrary.Controls.Interfaces;

public interface IContainerChildCompositor
{
    public RetroSpriteContainer? ParentContainer { get; }

    public void SetParentContainer(RetroSpriteContainer parentContainer);

    public Point GetChildPosition(RetroSpriteBase retroSpriteBase);
}