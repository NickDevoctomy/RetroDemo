using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RetroLibrary;

public class Button : IDisposable
{
    private SmartButtonTexture2D? _upSmartButtonTexture2D;
    private SmartButtonTexture2D? _downSmartButtonTexture2D;
    private Texture2D? _buttonUpTexture;
    private Texture2D? _buttonDownTexture;
    private SpriteFont _font;
    private bool _isHovered;
    private bool _isPressed;
    private bool disposedValue;

    public Vector2 Position { get; set; }

    public Vector2 Size { get; set; }

    public string Text { get; set; }

    public Color BackgroundColor { get; set; }

    public Color ForegroundColor { get; set; }


    public SpriteFont Font 
    { 
        get => _font;
        set => _font = value;
    }

    public Rectangle Bounds => new((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
    public bool IsHovered => _isHovered;

    public event EventHandler? Clicking;
    public event EventHandler? Released;
    public event EventHandler? Clicked;

    public Button(
        GraphicsDevice graphicsDevice,
        SpriteFont font,
        string text,
        Vector2 position,
        Vector2 size)
    {
        _font = font;
        Text = text;
        Position = position;
        Size = size;
        BackgroundColor = Color.Gray;
        ForegroundColor = Color.White;

        _upSmartButtonTexture2D = new SmartButtonTexture2D();
        _buttonUpTexture = _upSmartButtonTexture2D.BuildTexture(
            graphicsDevice,
            Texture2D.FromFile(graphicsDevice, "Content/Textures/greybuttonup.png"),
            (int)size.X,
            (int)size.Y,
            new SmartButtonOptions
            {
                TopMargin = 4,
                LeftMargin = 4,
                BottomMargin = 8,
                RightMargin = 4,
                Tint = BackgroundColor
            });

        _downSmartButtonTexture2D = new SmartButtonTexture2D();
        _buttonDownTexture = _downSmartButtonTexture2D.BuildTexture(
            graphicsDevice,
            Texture2D.FromFile(graphicsDevice, "Content/Textures/greybuttondown.png"),
            (int)size.X,
            (int)size.Y,
            new SmartButtonOptions
            {
                TopMargin = 6,
                LeftMargin = 4,
                BottomMargin = 6,
                RightMargin = 4,
                Tint = BackgroundColor
            });
    }

    public Button(
        GraphicsDevice graphicsDevice,
        SpriteFont font,
        string text,
        Vector2 position,
        Vector2 size, 
        Color backgroundColor,
        Color foregroundColor) 
        : this(graphicsDevice, font, text, position, size)
    {
        BackgroundColor = backgroundColor;
        ForegroundColor = foregroundColor;
    }

    public void Update(
        MouseState mouseState,
        MouseState previousMouseState)
    {
        var mousePosition = new Point(mouseState.X, mouseState.Y);
        _isHovered = Bounds.Contains(mousePosition);
        if (_isHovered)
        {
            switch(previousMouseState.LeftButton)
            {
                case ButtonState.Pressed:
                    {
                        _isPressed = true;
                        OnClicking();   
                        break;
                    }

                case ButtonState.Released:
                    {
                        if(_isPressed)
                        {
                            _isPressed = false;
                            OnClick();
                            OnReleased();
                        }
                        break;
                    }
            }
        }
        else
        {
            _isPressed = false;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            _isPressed ? _buttonDownTexture : _buttonUpTexture,
            Bounds,
            BackgroundColor);
        if (_font != null && !string.IsNullOrEmpty(Text))
        {
            var yOffset = _isPressed ? 2 : 0;
            Vector2 textSize = _font.MeasureString(Text);
            Vector2 textPosition = new Vector2(
                Position.X + (Size.X - textSize.X) / 2,
                Position.Y + yOffset + (Size.Y - textSize.Y) / 2
            );

            spriteBatch.DrawString(
                _font,
                Text,
                textPosition,
                ForegroundColor);
        }
    }

    protected virtual void OnClicking()
    {
        Clicking?.Invoke(
            this,
            EventArgs.Empty);
    }

    protected virtual void OnClick()
    {
        Clicked?.Invoke(
            this,
            EventArgs.Empty);
    }

    protected virtual void OnReleased()
    {
        Released?.Invoke(
            this,
            EventArgs.Empty);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            _upSmartButtonTexture2D?.Dispose();
            _upSmartButtonTexture2D = null;

            _downSmartButtonTexture2D?.Dispose();
            _downSmartButtonTexture2D = null;

            disposedValue = true;
        }
    }

    ~Button()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}