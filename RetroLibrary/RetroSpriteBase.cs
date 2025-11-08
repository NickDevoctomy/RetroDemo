using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RetroLibrary;

public partial class RetroSpriteBase : ObservableObject, IDisposable
{
    private readonly List<string> _watchedProperties = new List<string>();

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
    private Texture2D? _offscreenBuffer; // !!! Something is wrong with this buffer

    public RetroSpriteBase(
        string name,
        Point position,
        Point size,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        SpriteFont? font = null,
        bool buffered = true,
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

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!Buffered)
        {
            System.Diagnostics.Debug.WriteLine($"Drawing unbuffered button '{Name}'.");
            OnRedraw(spriteBatch, Position);
            return;
        }

        if (_needsRedraw || _offscreenBuffer == null)
        {
            System.Diagnostics.Debug.WriteLine($"Redrawing button '{Name}' buffer.");
            RedrawOffscreenBuffer(spriteBatch.GraphicsDevice);
        }

        ////System.Diagnostics.Debug.WriteLine($"Redrawing button '{Name}' from buffer.");
        if (_offscreenBuffer != null)
        {
            spriteBatch.Draw(
                _offscreenBuffer,
                new Rectangle(Position, Size),
                Color.White);
        }
    }

    public void Update(
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

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
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

    protected virtual void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
        // We don't draw anything in the base class
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (string.IsNullOrEmpty(e.PropertyName))
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
        _offscreenBuffer?.Dispose();
        _offscreenBuffer = null;

        var renderTarget = new RenderTarget2D(
            graphicsDevice,
            Size.X,
            Size.Y,
            false,
            SurfaceFormat.Color,
            DepthFormat.None,
            0,
            RenderTargetUsage.PlatformContents);

        using var spriteBatch = new SpriteBatch(graphicsDevice);

        var originalRenderTargets = graphicsDevice.GetRenderTargets();
        graphicsDevice.SetRenderTarget(renderTarget);
        graphicsDevice.Clear(BackgroundColor);
        spriteBatch.Begin();
        ////spriteBatch.DrawString(Font, "Arse", new Vector2(0, 0), Color.White); // This draws correctly
        OnRedraw(spriteBatch, Point.Zero);
        spriteBatch.End();
        graphicsDevice.SetRenderTargets(originalRenderTargets);

        _offscreenBuffer = renderTarget;
        _needsRedraw = false;
    }

    private void OnWatchedPropertyChanged(string propertyName)
    {
        _needsRedraw = true;
    }
}
