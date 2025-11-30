using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using RetroLibrary.Controls.Interfaces;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Enums;

namespace RetroLibrary.Controls.ContainerChildCompositors;

public class StackContainerChildCompositor : IContainerChildCompositor
{
    public RetroSpriteContainer? ParentContainer { get; private set; }

    public Direction Direction { get; set; } = Direction.Vertical;

    public Point GetChildPosition(RetroSpriteBase retroSpriteBase)
    {
        if (ParentContainer == null)
        {
            return Point.Zero;
        }

        var currentIndex = ParentContainer!.Children.IndexOf(retroSpriteBase);
        if (currentIndex == -1)
        {
            return Point.Zero;
        }

        var offset = 0;
        for (var i = 0; i < currentIndex; i++)
        {
            offset += Direction == Direction.Vertical
                ? ParentContainer!.Children[i].Size.Y
                : ParentContainer!.Children[i].Size.X;
        }

        return Direction == Direction.Vertical
            ? new Point(0, offset)
            : new Point(offset, 0);
    }

    public void SetParentContainer(RetroSpriteContainer parentContainer)
    {
        ParentContainer = parentContainer;
    }

    public void SetProperties(IDictionary<string, string> properties)
    {
        if (properties.TryGetValue("Direction", out var directionValue))
        {
            Direction = Enum.Parse<Direction>(directionValue.ToString() ?? "Vertical");
        }
    }
}