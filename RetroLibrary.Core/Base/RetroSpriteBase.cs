using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroLibrary.Core.Extensions;

namespace RetroLibrary.Core.Base;

public partial class RetroSpriteBase : ObservableObject, IDisposable
{
    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private Point position;

    [ObservableProperty]
    private Point size;

    [ObservableProperty]
    private Rectangle margins;

    [ObservableProperty]
    private Rectangle padding;

    [ObservableProperty]
    private Color backgroundColor;

    [ObservableProperty]
    private Color foregroundColor;

    [ObservableProperty]
    private SpriteFont? font;

    [ObservableProperty]
    private bool isPressed;

    [ObservableProperty]
    private bool isHovered;

    [ObservableProperty]
    private bool isVisible;

    private RenderTarget2D? _offscreenRenderTarget;

    public RetroSpriteBase(
        string name,
        Point position,
        Point size,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        SpriteFont? font = null,
        Rectangle? margins = null,
        Rectangle? padding = null,
        bool isVisible = true)
    {
        Name = name;
        Position = position;
        Size = size;
        BackgroundColor = backgroundColor ?? Color.Transparent;
        ForegroundColor = foregroundColor ?? Color.Black;
        Font = font;
        Margins = margins ?? Rectangle.Empty;
        Padding = padding ?? Rectangle.Empty;
        IsVisible = isVisible;
    }

    public event EventHandler? Clicking;

    public event EventHandler? Released;

    public event EventHandler? Clicked;

    public RetroGameContext? RetroGameContext { get; private set; }

    public void Init(RetroGameContext retroGameContext)
    {
        RetroGameContext = retroGameContext;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsVisible)
        {
            return;
        }

        OnRedraw(spriteBatch, Position);
    }

    public void DrawAtPosition(SpriteBatch spriteBatch, Point position)
    {
        if (!IsVisible)
        {
            return;
        }

        OnRedraw(spriteBatch, position);
    }

    public void Update(
            MouseState mouseState,
            MouseState previousMouseState)
    {
        if (!IsVisible)
        {
            return;
        }

        OnUpdate(mouseState, previousMouseState);
    }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);

        _offscreenRenderTarget?.Dispose();
        _offscreenRenderTarget = null;
    }

    protected virtual void OnUpdate(
        MouseState mouseState,
        MouseState previousMouseState)
    {
        var bounds = new Rectangle(Position, Size);
        IsHovered = bounds.Contains(mouseState.Position);
        if (IsHovered)
        {
            switch (previousMouseState.LeftButton)
            {
                case ButtonState.Pressed:
                    {
                        IsPressed = true;
                        OnClicking();
                        break;
                    }

                case ButtonState.Released:
                    {
                        if (IsPressed)
                        {
                            OnClicked();
                            OnReleased();
                            IsPressed = false;
                        }

                        break;
                    }
            }
        }
        else
        {
            IsPressed = false;
            OnReleased();
        }
    }

    protected virtual void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
        // We don't draw anything in the base class
    }

    protected virtual void OnClicking()
    {
        Clicking?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnClicked()
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnReleased()
    {
        Released?.Invoke(this, EventArgs.Empty);
    }

    protected Rectangle GetBounds()
    {
        var source = new Rectangle(Position, Size);
        return source.GetBounds(Margins, Padding);
    }
}