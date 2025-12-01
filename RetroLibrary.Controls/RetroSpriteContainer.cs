using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroLibrary.Controls.ContainerChildCompositors;
using RetroLibrary.Controls.Interfaces;
using RetroLibrary.Core.Base;

namespace RetroLibrary.Controls;

public partial class RetroSpriteContainer : RetroSpriteBase
{
    private readonly HashSet<RetroSpriteBase> _subscribedChildren = [];

    [ObservableProperty]
    private List<RetroSpriteBase> children = [];

    [ObservableProperty]
    private Rectangle innerPadding;

    [ObservableProperty]
    private int childrenChangeVersion;

    [ObservableProperty]
    private IContainerChildCompositor? childCompositor;

    public RetroSpriteContainer(
        string name,
        Point position,
        Point size,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        Rectangle? innerPadding = null,
        SpriteFont? font = null,
        IContainerChildCompositor? childCompositor = null,
        Rectangle? margins = null,
        bool isVisible = true)
        : base(
            name,
            position,
            size,
            backgroundColor,
            foregroundColor,
            font,
            margins,
            isVisible)
    {
        InnerPadding = innerPadding ?? Rectangle.Empty;
        ChildCompositor = childCompositor ?? new ChildOwnusContainerChildCompositor();
        ChildCompositor.SetParentContainer(this);
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
        var graphicsDevice = spriteBatch.GraphicsDevice;
        var previousScissor = graphicsDevice.ScissorRectangle;
        var clipRect = new Rectangle(
            location.X + InnerPadding.X,
            location.Y + InnerPadding.Y,
            Size.X - (InnerPadding.X + InnerPadding.Width),
            Size.Y - (InnerPadding.Y + InnerPadding.Height));
        clipRect = Rectangle.Intersect(previousScissor, clipRect);
        graphicsDevice.ScissorRectangle = clipRect;

        DrawChildren(spriteBatch, location);

        graphicsDevice.ScissorRectangle = previousScissor;
    }

    protected override void OnUpdate(
        MouseState mouseState,
        MouseState previousMouseState)
    {
        base.OnUpdate(
            mouseState,
            previousMouseState);

        foreach (var currentChild in Children)
        {
            var offsetMouseState = new MouseState(
                mouseState.X - (Position.X + InnerPadding.X),
                mouseState.Y - (Position.Y + InnerPadding.Y),
                mouseState.ScrollWheelValue,
                mouseState.LeftButton,
                mouseState.MiddleButton,
                mouseState.RightButton,
                mouseState.XButton1,
                mouseState.XButton2);

            var offsetPreviousMouseState = new MouseState(
                previousMouseState.X - (Position.X + InnerPadding.X),
                previousMouseState.Y - (Position.Y + InnerPadding.Y),
                previousMouseState.ScrollWheelValue,
                previousMouseState.LeftButton,
                previousMouseState.MiddleButton,
                previousMouseState.RightButton,
                previousMouseState.XButton1,
                previousMouseState.XButton2);

            currentChild.Update(
                offsetMouseState,
                offsetPreviousMouseState);
        }
    }

    protected void DrawChildren(SpriteBatch spriteBatch, Point location)
    {
        foreach (var currentChild in Children)
        {
            var position = ChildCompositor!.GetChildPosition(currentChild);
            position += location + InnerPadding.Location; // Container offsets
            currentChild.DrawAtPosition(spriteBatch, position);
        }
    }
}