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
        bool buffered = false,
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
        Texture = texture;
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
        Texture?.Draw(
                Size.X,
                Size.Y,
                spriteBatch,
                new Rectangle(
                    location,
                    Size));
    }
}
