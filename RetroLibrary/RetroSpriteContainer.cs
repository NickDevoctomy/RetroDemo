using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RetroLibrary;

public partial class RetroSpriteContainer : RetroSpriteBase
{
    [ObservableProperty]
    private List<RetroSpriteBase> children = new ();

    [ObservableProperty]
    private Rectangle innerMargins;

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
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
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
}
