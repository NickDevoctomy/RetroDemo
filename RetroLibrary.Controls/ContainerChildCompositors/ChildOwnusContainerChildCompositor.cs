using Microsoft.Xna.Framework;
using RetroLibrary.Controls.Interfaces;
using RetroLibrary.Core.Base;

namespace RetroLibrary.Controls.ContainerChildCompositors;

public class ChildOwnusContainerChildCompositor : IContainerChildCompositor
{
    public RetroSpriteContainer? ParentContainer { get; private set; }


    public Point GetChildPosition(RetroSpriteBase retroSpriteBase)
    {
        return retroSpriteBase.Position;
    }

    public void SetParentContainer(RetroSpriteContainer parentContainer)
    {
        ParentContainer = parentContainer;
    }
}