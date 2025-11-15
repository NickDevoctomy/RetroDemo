using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RetroLibrary;

public partial class RetroSpriteContainer : RetroSpriteBase
{
    private readonly HashSet<RetroSpriteBase> _subscribedChildren = new ();

    [ObservableProperty]
    private List<RetroSpriteBase> children = new ();

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
        bool buffered = true,
        bool updateWatchedProperties = true)
        : base(
            name,
            position,
            size,
            backgroundColor,
            foregroundColor,
            font,
            buffered,
            updateWatchedProperties)
    {
        InnerMargins = innerMargins ?? Rectangle.Empty;
        EnsureChildSubscriptions();
    }

    public override void SetWatchedProperties(List<string> propertyNames)
    {
        base.SetWatchedProperties(propertyNames);
        propertyNames.Add(nameof(ChildrenChangeVersion));
        propertyNames.Add(nameof(InnerMargins));
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
        // Keep subscriptions current before drawing
        EnsureChildSubscriptions();
        var childrenTexture = DrawChildrenToTexture(spriteBatch.GraphicsDevice);

        var innerLocation = new Point(
            location.X + InnerMargins.X,
            location.Y + InnerMargins.Y);

        var innerSize = new Point(
            Size.X - InnerMargins.X - InnerMargins.Width,
            Size.Y - InnerMargins.Y - InnerMargins.Height);

        spriteBatch.Draw(
            childrenTexture,
            new Rectangle(innerLocation, innerSize),
            Color.White);
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

    protected Texture2D DrawChildrenToTexture(GraphicsDevice graphicsDevice)
    {
        var renderTarget = new RenderTarget2D(
            graphicsDevice,
            Size.X - (InnerMargins.X + InnerMargins.Width),
            Size.Y - (InnerMargins.Y + InnerMargins.Height),
            false,
            SurfaceFormat.Color,
            DepthFormat.None,
            0,
            RenderTargetUsage.PlatformContents);

        using var spriteBatch = new SpriteBatch(graphicsDevice);

        var originalRenderTargets = graphicsDevice.GetRenderTargets();
        graphicsDevice.SetRenderTarget(renderTarget);
        graphicsDevice.Clear(Color.Transparent);
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

        foreach (var currentChild in Children)
        {
            currentChild.Draw(spriteBatch);
        }

        spriteBatch.End();
        graphicsDevice.SetRenderTargets(originalRenderTargets);

        return renderTarget;
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
