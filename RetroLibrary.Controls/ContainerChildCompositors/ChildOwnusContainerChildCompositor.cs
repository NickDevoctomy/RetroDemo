using Microsoft.Xna.Framework;
using RetroLibrary.Controls.Interfaces;
using RetroLibrary.Core.Base;

namespace RetroLibrary.Controls.ContainerChildCompositors;

public class ChildOwnusContainerChildCompositor : IContainerChildCompositor
{
    public RetroSpriteContainer? ParentContainer { get; private set; }


    public Point GetChildPosition(RetroSpriteBase retroSpriteBase)
    {
        var bounds = retroSpriteBase.GetBounds();
        return bounds.Location;
    }

    public void SetParentContainer(RetroSpriteContainer parentContainer)
    {
        ParentContainer = parentContainer;
    }

    public void SetProperties(IDictionary<string, string> properties)
    {
        // Do nothing
    }
}