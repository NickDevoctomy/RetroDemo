using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RetroLibrary.Core.Base;

public partial class RetroSpriteBase : ObservableObject, IDisposable
{
    ////private readonly List<string> _watchedProperties = [];

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private Point position;

    [ObservableProperty]
    private Point size;

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
        bool isVisible = true,
        bool updateWatchedProperties = true)
    {
        Name = name;
        Position = position;
        Size = size;
        BackgroundColor = backgroundColor ?? Color.Transparent;
        ForegroundColor = foregroundColor ?? Color.Black;
        Font = font;
        IsVisible = isVisible;

        ////if (updateWatchedProperties)
        ////{
        ////    UpdateWatchedProperties();
        ////}
    }

    public event EventHandler? Clicking;

    public event EventHandler? Released;

    public event EventHandler? Clicked;

    public event EventHandler? WatchedPropertyChanged;

    public RetroGameContext? RetroGameContext { get; private set; }

    public void Init(RetroGameContext retroGameContext)
    {
        RetroGameContext = retroGameContext;
    }

    public void Draw(SpriteBatch spriteBatch, Point? location = null)
    {
        if (!IsVisible)
        {
            return;
        }

        OnRedraw(spriteBatch, (location ?? Point.Zero) + Position);
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

    ////public void UpdateWatchedProperties()
    ////{
    ////    SetWatchedProperties(_watchedProperties);
    ////}

    ////public virtual void SetWatchedProperties(List<string> propertyNames)
    ////{
    ////    _watchedProperties.Add(nameof(Size));
    ////    _watchedProperties.Add(nameof(IsPressed));
    ////}

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
}