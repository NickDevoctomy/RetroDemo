using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RetroLibrary.Core.Base;

public partial class RetroSpriteBase : ObservableObject, IDisposable
{
    private readonly List<string> _watchedProperties = new ();

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
    private bool buffered;

    [ObservableProperty]
    private bool isPressed;

    [ObservableProperty]
    private bool isHovered;

    private bool _needsRedraw = true;
    private RenderTarget2D? _offscreenRenderTarget;

    public RetroSpriteBase(
        string name,
        Point position,
        Point size,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        SpriteFont? font = null,
        bool buffered = false,
        bool updateWatchedProperties = true)
    {
        Name = name;
        Position = position;
        Size = size;
        BackgroundColor = backgroundColor ?? Color.Transparent;
        ForegroundColor = foregroundColor ?? Color.Black;
        Font = font;
        Buffered = buffered;

        if (updateWatchedProperties)
        {
            UpdateWatchedProperties();
        }
    }

    public event EventHandler? Clicking;

    public event EventHandler? Released;

    public event EventHandler? Clicked;

    public event EventHandler? WatchedPropertyChanged;

    public void Draw(SpriteBatch spriteBatch, Point? location = null)
    {
        if (!Buffered)
        {
            System.Diagnostics.Debug.WriteLine($"Drawing unbuffered sprite '{Name}'.");
            OnRedraw(spriteBatch, (location ?? Point.Zero) + Position);
            return;
        }

        throw new NotImplementedException("Buffered drawing is not implemented yet.");

        if (_needsRedraw || _offscreenRenderTarget == null)
        {
            System.Diagnostics.Debug.WriteLine($"Redrawing offscreen buffer for '{Name}'.");
            RedrawOffscreenBuffer(spriteBatch.GraphicsDevice);
        }

        if (_offscreenRenderTarget != null)
        {
            spriteBatch.Draw(
                _offscreenRenderTarget,
                new Rectangle(Position, Size),
                Color.White);
        }
    }

    public void Update(
            MouseState mouseState,
            MouseState previousMouseState)
    {
        OnUpdate(mouseState, previousMouseState);
    }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);

        _offscreenRenderTarget?.Dispose();
        _offscreenRenderTarget = null;
    }

    public void UpdateWatchedProperties()
    {
        SetWatchedProperties(_watchedProperties);
    }

    public virtual void SetWatchedProperties(List<string> propertyNames)
    {
        _watchedProperties.Add(nameof(Size));
        _watchedProperties.Add(nameof(IsPressed));
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

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (!Buffered || string.IsNullOrEmpty(e.PropertyName))
        {
            return;
        }

        if (_watchedProperties.Contains(e.PropertyName))
        {
            OnWatchedPropertyChanged(e.PropertyName);
            _needsRedraw = true;
        }
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

    private void RedrawOffscreenBuffer(GraphicsDevice graphicsDevice)
    {
        _offscreenRenderTarget ??= new RenderTarget2D(
                graphicsDevice,
                Size.X,
                Size.Y,
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                0,
                RenderTargetUsage.PreserveContents);

        var originalRenderTargets = graphicsDevice.GetRenderTargets();
        System.Diagnostics.Debug.WriteLine($"{this.GetType().Name}: Switching render target.");
        graphicsDevice.SetRenderTarget(_offscreenRenderTarget);             // Switching render target causes flicker
        graphicsDevice.Clear(BackgroundColor);

        using var spriteBatch = new SpriteBatch(graphicsDevice);
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

        OnRedraw(spriteBatch, Point.Zero);

        spriteBatch.End();
        graphicsDevice.SetRenderTargets(originalRenderTargets);

        _needsRedraw = false;
    }

    private void OnWatchedPropertyChanged(string propertyName)
    {
        _needsRedraw = true;
        WatchedPropertyChanged?.Invoke(this, EventArgs.Empty);
    }
}
