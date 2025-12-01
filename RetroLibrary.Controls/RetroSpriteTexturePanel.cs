using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Drawing;

namespace RetroLibrary.Controls;

public partial class RetroSpriteTexturePanel : RetroSpriteBase
{
    [ObservableProperty]
    private IRetroTexture2D? texture;

    public RetroSpriteTexturePanel(
        string name,
        Point position,
        Point size,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        IRetroTexture2D? texture = null,
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
        Texture = texture;
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
        if (Texture is null)
        {
            return;
        }

        var source = new Rectangle(0, 0, Size.X, Size.Y);
        var bounds = GetBounds();

        Texture.Draw(
            Size.X,
            Size.Y,
            spriteBatch,
            source,
            bounds);
    }
}