using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroLibrary.Core.Base;

namespace RetroLibrary.Controls;

public partial class RetroSpriteContainer : RetroSpriteBase
{
    private readonly HashSet<RetroSpriteBase> _subscribedChildren = [];

    [ObservableProperty]
    private List<RetroSpriteBase> children = [];

    [ObservableProperty]
    private Rectangle innerMargins;

    [ObservableProperty]
    private int childrenChangeVersion;

    public RetroSpriteContainer(
        string name,
        Point position,
        Point size,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        Rectangle? innerMargins = null,
        SpriteFont? font = null,
        bool isVisible = true)
        : base(
            name,
            position,
            size,
            backgroundColor,
            foregroundColor,
            font,
            isVisible)
    {
        InnerMargins = innerMargins ?? Rectangle.Empty;
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
        var graphicsDevice = spriteBatch.GraphicsDevice;
        var previousScissor = graphicsDevice.ScissorRectangle;
        var clipRect = new Rectangle(
            location.X + InnerMargins.X,
            location.Y + InnerMargins.Y,
            Size.X - (InnerMargins.X + InnerMargins.Width),
            Size.Y - (InnerMargins.Y + InnerMargins.Height));
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
                mouseState.X - (Position.X + InnerMargins.X),
                mouseState.Y - (Position.Y + InnerMargins.Y),
                mouseState.ScrollWheelValue,
                mouseState.LeftButton,
                mouseState.MiddleButton,
                mouseState.RightButton,
                mouseState.XButton1,
                mouseState.XButton2);

            var offsetPreviousMouseState = new MouseState(
                previousMouseState.X - (Position.X + InnerMargins.X),
                previousMouseState.Y - (Position.Y + InnerMargins.Y),
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
            currentChild.Draw(spriteBatch, location + InnerMargins.Location);
        }
    }
}