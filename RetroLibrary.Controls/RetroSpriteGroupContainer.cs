using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Core.Drawing;

namespace RetroLibrary.Controls;

public partial class RetroSpriteGroupContainer : RetroSpriteContainer
{
    [ObservableProperty]
    private string text = string.Empty;

    [ObservableProperty]
    private int borderOuterTopMargin;

    [ObservableProperty]
    private NineSliceTexture2D? borderTexture;

    [ObservableProperty]
    private NineSliceTexture2D? groupLabelTexture;

    [ObservableProperty]
    private Color? borderTint;

    [ObservableProperty]
    private Color? labelTint;

    [ObservableProperty]
    private Point labelOffset;

    public RetroSpriteGroupContainer(
        string name,
        string text,
        Point position,
        Point size,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        NineSliceTexture2D? borderTexture = null,
        int borderOuterTopMargin = 0,
        NineSliceTexture2D? groupLabelTexture = null,
        Color? borderTint = null,
        Color? labelTint = null,
        Point? labelOffset = null,
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
            innerMargins,
            font,
            buffered,
            updateWatchedProperties)
    {
        Text = text;
        BorderTexture = borderTexture;
        BorderOuterTopMargin = borderOuterTopMargin;
        GroupLabelTexture = groupLabelTexture;
        BorderTint = borderTint;
        LabelTint = labelTint;
        LabelOffset = labelOffset ?? new Point(8, 0);
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
        if (BorderTexture != null)
        {
            var borderLocation = new Point(
                location.X,
                location.Y + BorderOuterTopMargin);
            var borderSize = new Point(
                Size.X,
                Size.Y - BorderOuterTopMargin);
            var borderTexture = BorderTexture.BuildTexture(
                spriteBatch.GraphicsDevice,
                borderSize.X,
                borderSize.Y);
            spriteBatch.Draw(
                borderTexture,
                new Rectangle(borderLocation, borderSize),
                BorderTint ?? Color.White);
        }

        if (!string.IsNullOrEmpty(Text) && Font != null)
        {
            Vector2 textSize = Font.MeasureString(Text);
            int labelWidth = (int)textSize.X + 16;
            int labelHeight = (int)textSize.Y + 4;

            if (GroupLabelTexture != null)
            {
                var labelTexture = GroupLabelTexture.BuildTexture(
                    spriteBatch.GraphicsDevice,
                    labelWidth,
                    labelHeight);
                spriteBatch.Draw(
                    labelTexture,
                    new Rectangle(LabelOffset.X, LabelOffset.Y, labelWidth, labelHeight),
                    LabelTint ?? Color.White);
            }

            Vector2 textPosition = new (
                LabelOffset.X + ((labelWidth - textSize.X) / 2),
                LabelOffset.Y + ((labelHeight - textSize.Y) / 2) + 2); // Hardcoded additional offset for current font but we really want this to be settable
            spriteBatch.DrawString(
                Font,
                Text,
                textPosition,
                ForegroundColor);
        }

        base.OnRedraw(
            spriteBatch,
            location);
    }
}
