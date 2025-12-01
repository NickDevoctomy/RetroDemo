using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroLibrary.Core.Attributes;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Binding;
using RetroLibrary.Core.Drawing;
using RetroLibrary.Core.Enums;

namespace RetroLibrary.Controls;

public partial class RetroSpriteSliderBar : RetroSpriteBase
{
    [ObservableProperty]
    private NineSliceTexture2D? _sliderBarTexture;

    [ObservableProperty]
    private Color _sliderBarTint;

    [ObservableProperty]
    private float minimum = 0;

    [ObservableProperty]
    private float maximum = 100;

    [RetroSpriteBindableProperty]
    [ObservableProperty]
    private BindingValue<float> value = new(50);

    [ObservableProperty]
    private float buttonMarginLeft = 8;

    [ObservableProperty]
    private float buttonMarginRight = 8;

    [ObservableProperty]
    private NineSliceTexture2D? buttonTexture;

    [ObservableProperty]
    private Color buttonTint = Color.White;

    [ObservableProperty]
    private Color buttonHoverTint = Color.White;

    [ObservableProperty]
    private int barHeight = 8;

    [ObservableProperty]
    private int buttonWidth = 16;

    [ObservableProperty]
    private int buttonHeight = 0;

    [ObservableProperty]
    private bool isThumbHovered;

    [ObservableProperty]
    private ValueFrequency valueFrequency = ValueFrequency.Integer;

    private bool _isDragging;

    public RetroSpriteSliderBar(
        string name,
        Point position,
        Point size,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        BindingValue<float>? value = null,
        NineSliceTexture2D? sliderBarTexture = null,
        Color? sliderBarTint = null,
        NineSliceTexture2D? buttonTexture = null,
        Color? buttonTint = null,
        Color? buttonHoverTint = null,
        ValueFrequency valueFrequency = ValueFrequency.Integer,
        SpriteFont? font = null,
        Rectangle? margins = null,
        Rectangle? padding = null,
        bool isVisible = true)
        : base(
            name,
            position,
            size,
            backgroundColor,
            foregroundColor,
            font,
            margins,
            padding,
            isVisible)
    {
        Value = value ?? new BindingValue<float>(50);
        SliderBarTexture = sliderBarTexture;
        SliderBarTint = sliderBarTint ?? Color.White;
        ButtonTexture = buttonTexture;
        ButtonTint = buttonTint ?? Color.White;
        ButtonHoverTint = buttonHoverTint ?? ButtonTint;
        ValueFrequency = valueFrequency;
        if (this.valueFrequency == ValueFrequency.Integer)
        {
            Value.SetValue(Math.Round(Value.Value, 0));
        }
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        SliderBarTexture?.Dispose();
        SliderBarTexture = null;
        ButtonTexture?.Dispose();
        ButtonTexture = null;
    }

    protected override void OnRedraw(SpriteBatch spriteBatch, Point location)
    {
        int barH = BarHeight > 0 ? BarHeight : 8;
        if (SliderBarTexture is not null)
        {
            var sliderBarTexture = SliderBarTexture.BuildTexture(
                spriteBatch.GraphicsDevice,
                Size.X,
                barH);

            var rect = new Rectangle(
                location.X,
                location.Y + (Size.Y / 2) - (sliderBarTexture.Height / 2),
                Size.X,
                sliderBarTexture.Height);

            spriteBatch.Draw(
                sliderBarTexture,
                rect,
                SliderBarTint);
        }

        var thumbRect = GetThumbRectangle(location);
        if (ButtonTexture is not null && thumbRect.Width > 0 && thumbRect.Height > 0)
        {
            var thumbTex = ButtonTexture.BuildTexture(
                spriteBatch.GraphicsDevice,
                thumbRect.Width,
                thumbRect.Height);

            var tint = (_isDragging || IsThumbHovered) ? ButtonHoverTint : ButtonTint;
            spriteBatch.Draw(
                thumbTex,
                thumbRect,
                tint);
        }
    }

    protected override void OnUpdate(MouseState mouseState, MouseState previousMouseState)
    {
        base.OnUpdate(mouseState, previousMouseState);

        var localMouse = new Point(mouseState.X - Position.X, mouseState.Y - Position.Y);
        bool mouseWentDown = previousMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed;
        bool mouseHeld = mouseState.LeftButton == ButtonState.Pressed;
        bool mouseReleased = previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released;

        IsThumbHovered = GetThumbRectangle(Point.Zero).Contains(localMouse);

        if (mouseWentDown)
        {
            if (IsThumbHovered)
            {
                _isDragging = true;
            }
            else if (localMouse.Y >= 0 && localMouse.Y <= Size.Y && localMouse.X >= 0 && localMouse.X <= Size.X)
            {
                UpdateValueFromMouseX(localMouse.X);
                _isDragging = true;
            }
        }

        if (_isDragging && mouseHeld)
        {
            UpdateValueFromMouseX(localMouse.X);
        }

        if (_isDragging && mouseReleased)
        {
            _isDragging = false;
        }
    }

    protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (string.IsNullOrEmpty(e.PropertyName))
        {
            return;
        }

        if (e.PropertyName == nameof(ValueFrequency))
        {
            if (ValueFrequency == ValueFrequency.Integer)
            {
                Value.SetValue(ClampAndRound(Value.Value));
            }
        }
        else if (e.PropertyName == nameof(Value))
        {
            if (ValueFrequency == ValueFrequency.Integer)
            {
                Value.SetValue(ClampAndRound(Value.Value));
            }
            else
            {
                Value.SetValue(MathHelper.Clamp(Value.Value, Minimum, Maximum));
            }
        }
        else if (e.PropertyName == nameof(Minimum) || e.PropertyName == nameof(Maximum))
        {
            if (ValueFrequency == ValueFrequency.Integer)
            {
                Value.SetValue(ClampAndRound(Value.Value));
            }
            else
            {
                Value.SetValue(MathHelper.Clamp(Value.Value, Minimum, Maximum));
            }
        }
    }

    private float ClampAndRound(float v)
    {
        var clamped = MathHelper.Clamp(v, Minimum, Maximum);
        return (float)Math.Round(clamped);
    }

    private Rectangle GetThumbRectangle(Point location)
    {
        int thumbW = ButtonWidth > 0 ? ButtonWidth : 16;
        int thumbH = ButtonHeight > 0 ? ButtonHeight : Size.Y;

        float minCenterX = ButtonMarginLeft + (thumbW / 2f);
        float maxCenterX = Size.X - ButtonMarginRight - (thumbW / 2f);
        float travel = MathF.Max(0, maxCenterX - minCenterX);

        float range = MathF.Max(0.0001f, Maximum - Minimum);
        float t = MathHelper.Clamp((Value.Value - Minimum) / range, 0f, 1f);

        float centerX = minCenterX + (t * travel);

        int left = (int)MathF.Round(centerX - (thumbW / 2f));
        int top = location.Y + ((Size.Y - thumbH) / 2);

        return new Rectangle(location.X + left, top, thumbW, thumbH);
    }

    private void UpdateValueFromMouseX(int localMouseX)
    {
        int thumbW = ButtonWidth > 0 ? ButtonWidth : 16;

        float minCenterX = ButtonMarginLeft + (thumbW / 2f);
        float maxCenterX = Size.X - ButtonMarginRight - (thumbW / 2f);
        float travel = MathF.Max(0, maxCenterX - minCenterX);

        float clampedCenter = MathHelper.Clamp(localMouseX, minCenterX, maxCenterX);
        float t = travel <= 0 ? 0f : (clampedCenter - minCenterX) / travel;

        float newValue = Minimum + (t * (Maximum - Minimum));
        if (ValueFrequency == ValueFrequency.Integer)
        {
            newValue = (float)Math.Round(newValue);
        }

        Value.SetValue(newValue);
    }
}