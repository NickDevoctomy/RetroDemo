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
        bool isVisible = true,
        bool updateWatchedProperties = true)
        : base(
            name,
            position,
            size,
            backgroundColor,
            foregroundColor,
            font,
            isVisible,
            updateWatchedProperties)
    {
        InnerMargins = innerMargins ?? Rectangle.Empty;
        EnsureChildSubscriptions();
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
        EnsureChildSubscriptions();

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

        EnsureChildSubscriptions();

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

    private void EnsureChildSubscriptions()
    {
        foreach (var child in Children)
        {
            if (_subscribedChildren.Contains(child))
            {
                continue;
            }

            child.WatchedPropertyChanged += Child_WatchedPropertyChanged;
            _subscribedChildren.Add(child);
        }

        var removed = _subscribedChildren.Where(c => !Children.Contains(c)).ToList();
        foreach (var child in removed)
        {
            child.WatchedPropertyChanged -= Child_WatchedPropertyChanged;
            _subscribedChildren.Remove(child);
        }
    }

    private void Child_WatchedPropertyChanged(object? sender, EventArgs e)
    {
        ChildrenChangeVersion++;
    }
}