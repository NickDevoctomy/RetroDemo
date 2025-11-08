using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;

namespace RetroLibrary;

public partial class RetroSpriteBase : ObservableObject, IDisposable
{
    public event EventHandler? Clicking;
    public event EventHandler? Released;
    public event EventHandler? Clicked;

    [ObservableProperty]
    private Point position;

    [ObservableProperty]
    private Point size;

    [ObservableProperty]
    private Color backgroundColor = Color.Transparent;

    [ObservableProperty]
    private Color foregroundColor = Color.Black;

    [ObservableProperty]
    private bool buffered;

    [ObservableProperty]
    private bool isPressed;

    private List<string> _watchedProperties = [];
    private bool _needsRedraw = true;
    private Texture2D? _offscreenBuffer;
    protected bool _isHovered { get; private set; }

    public RetroSpriteBase()
    {
        SetWatchedProperties(_watchedProperties);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!Buffered)
        {
            OnRedraw(spriteBatch, Position);
        }

        if(_needsRedraw)
        {
            RedrawOffscreenBuffer(spriteBatch.GraphicsDevice);
        }

        spriteBatch.Draw(
            _offscreenBuffer!,
            new Rectangle(Position, Size),
            Color.White);
    }

    public void Update(
            MouseState mouseState,
            MouseState previousMouseState)
    {
        var bounds = new Rectangle(Position, Size);
        _isHovered = bounds.Contains(mouseState.Position);
        if (_isHovered)
        {
            switch (previousMouseState.LeftButton)
            {
                case ButtonState.Pressed:
                    {
                        IsPressed = true;
                        Clicking?.Invoke(this, EventArgs.Empty);
                        break;
                    }

                case ButtonState.Released:
                    {
                        if (IsPressed)
                        {
                            IsPressed = false;
                            Clicked?.Invoke(this, EventArgs.Empty);
                            Released?.Invoke(this, EventArgs.Empty);
                        }
                        break;
                    }
            }
        }
        else
        {
            IsPressed = false;
            Released?.Invoke(this, EventArgs.Empty);
        }
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

    private void RedrawOffscreenBuffer(GraphicsDevice graphicsDevice)
    {
        var renderTarget = new RenderTarget2D(graphicsDevice, Size.X, Size.Y);
        var spriteBatch = new SpriteBatch(graphicsDevice);

        graphicsDevice.SetRenderTarget(renderTarget);
        graphicsDevice.Clear(BackgroundColor);
        spriteBatch.Begin();
        OnRedraw(spriteBatch, Point.Zero);
        spriteBatch.End();
        graphicsDevice.SetRenderTarget(null);
        spriteBatch.Dispose();

        _offscreenBuffer = renderTarget;
        _needsRedraw = false;
    }

    private void OnWatchedPropertyChanged(string propertyName)
    {
        _needsRedraw = true;
    }

    public virtual void Dispose()
    {
        throw new NotImplementedException();
    }
}
