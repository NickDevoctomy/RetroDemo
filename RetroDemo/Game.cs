using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroLibrary.Controls;
using RetroLibrary.Core;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Drawing;
using RetroLibrary.Core.Interfaces;
using RetroLibrary.Core.Resources;

namespace RetroDemo;

public class Game : RetroGameBase
{
    private readonly Random _rnd = new(Environment.TickCount);

    private Texture2DResourceLoader? _texture2DLoader;
    private SpriteFont? _font;
    private RetroSpriteContainer? _testContainer;
    private RetroSpriteNineSliceButton? _testButton;
    private RetroSpriteLabel? _testProgressBarLabel;
    private RetroSpriteProgressBar? _testProgressBar;
    private RetroSpriteCheckBox? _testCheckBox;
    private RetroSpriteSliderBar? _testSliderBar;
    private RetroSpriteTabbedContainer? _tabbedContainer;
    private RetroSpriteNineSliceButton? _orangeButton;
    private RetroSpriteNineSliceButton? _exitButton;
    private MiniParallaxScroller? _parallaxScroller;
    private RadialRetroGradientTexture2D? _radialGradientTexture;
    private MouseState _previousMouseState;

    public Game(RetroGameContext retroGameContext)
        : base(retroGameContext)
    {
    }

    protected override void OnLoadContent()
    {
        _texture2DLoader = new Texture2DResourceLoader();

        try
        {
            _font = Content.Load<SpriteFont>("DefaultFont");
        }
        catch
        {
            _font = null;
        }

        _testContainer = new RetroSpriteGroupContainer(
            "TestGroupContainer",
            "Some really long text in here!",
            new Point(8, 8),
            new Point(380, 250),
            borderTexture: new NineSliceTexture2D(
                _texture2DLoader.FromFile(
                    GraphicsDevice,
                    "Content/Textures/surface.png"),
                new NineSliceTextureOptions
                {
                    TopMargin = 4,
                    LeftMargin = 4,
                    BottomMargin = 4,
                    RightMargin = 4
                }),
            borderOuterTopMargin: 8,
            groupLabelTexture: new NineSliceTexture2D(
                _texture2DLoader.FromFile(
                    GraphicsDevice,
                    "Content/Textures/surface.png"),
                new NineSliceTextureOptions
                {
                    TopMargin = 4,
                    LeftMargin = 4,
                    BottomMargin = 4,
                    RightMargin = 4
                }),
            innerMargins: new Rectangle(8, 32, 8, 8),
            font: _font,
            borderTint: new Color(Color.LightGray, 0.5f),
            labelTint: Color.Red,
            foregroundColor: Color.White);

        _testButton = new RetroSpriteNineSliceButton(
            "TestButton",
            "Toggle Button",
            new Point(0, 0),
            new Point(200, 50),
            true,
            foregroundColor: Color.White,
            upTint: Color.LightGray,
            downTint: Color.Red,
            upTexture: new NineSliceTexture2D(
                _texture2DLoader.FromFile(
                    GraphicsDevice,
                    "Content/Textures/greybuttonup.png"),
                new NineSliceTextureOptions
                {
                    TopMargin = 4,
                    LeftMargin = 4,
                    BottomMargin = 8,
                    RightMargin = 4
                }),
            downTexture: new NineSliceTexture2D(
                _texture2DLoader.FromFile(
                    GraphicsDevice,
                    "Content/Textures/greybuttondown.png"),
                new NineSliceTextureOptions
                {
                    TopMargin = 6,
                    LeftMargin = 4,
                    BottomMargin = 6,
                    RightMargin = 4
                }),
            font: _font);
        _testButton.Clicked += TestButton_Clicked;
        _testButton.Clicking += TestButton_Clicking;

        _testProgressBarLabel = new RetroSpriteLabel(
            "TestProgressBarLabel",
            "Progress Bar:",
            new Point(0, 50),
            new Point(200, 32),
            foregroundColor: Color.Black,
            font: _font);

        _testProgressBar = new RetroSpriteProgressBar(
            "TestProgressBar",
            0.5f,
            new Point(0, 83),
            new Point(200, 32),
            borderTexture: new NineSliceTexture2D(
                _texture2DLoader.FromFile(
                    GraphicsDevice,
                    "Content/Textures/border.png"),
                new NineSliceTextureOptions
                {
                    TopMargin = 4,
                    LeftMargin = 4,
                    BottomMargin = 4,
                    RightMargin = 4
                }),
            borderTint: Color.Red);

        _testCheckBox = new RetroSpriteCheckBox(
            "TestCheckBox",
            "Check Me!",
            new Point(0, 120),
            new Point(150, 32),
            boxTexture: new NineSliceTexture2D(
                _texture2DLoader.FromFile(
                    GraphicsDevice,
                    "Content/Textures/surface.png"),
                new NineSliceTextureOptions
                {
                    TopMargin = 4,
                    LeftMargin = 4,
                    BottomMargin = 4,
                    RightMargin = 4
                }),
            isChecked: false,
            foregroundColor: Color.Black,
            font: _font);

        _tabbedContainer = new RetroSpriteTabbedContainer(
            "TabbedContainer",
            new Point(25, 35),
            new Point(400, 300),
            foregroundColor: Color.White,
            font: _font);
        _tabbedContainer.TabPageTexture = new NineSliceTexture2D(
                _texture2DLoader.FromFile(
                    GraphicsDevice,
                    "Content/Textures/surfacegrey.png"),
                new NineSliceTextureOptions
                {
                    TopMargin = 4,
                    LeftMargin = 4,
                    BottomMargin = 4,
                    RightMargin = 4
                });
        _tabbedContainer.TabPageTint = new Color(Color.LightGray, 0.5f);
        _tabbedContainer.TabUpTexture = new NineSliceTexture2D(
                _texture2DLoader.FromFile(
                    GraphicsDevice,
                    "Content/Textures/tab.png"),
                new NineSliceTextureOptions
                {
                    TopMargin = 4,
                    LeftMargin = 4,
                    BottomMargin = 4,
                    RightMargin = 4
                });
        _tabbedContainer.TabUpTint = new Color(Color.LightGray, 0.1f);
        _tabbedContainer.TabDownTexture = new NineSliceTexture2D(
                _texture2DLoader.FromFile(
                    GraphicsDevice,
                    "Content/Textures/tab.png"),
                new NineSliceTextureOptions
                {
                    TopMargin = 4,
                    LeftMargin = 4,
                    BottomMargin = 4,
                    RightMargin = 4
                });
        _tabbedContainer.TabDownTint = new Color(Color.LightGray, 0.5f);
        _tabbedContainer.TabPages.Add(new TabPage("Apple", [ _testContainer ]));

        _testSliderBar = new RetroSpriteSliderBar(
            "TestSliderBar",
            new Point(8, 8),
            new Point(350, 32),
            sliderBarTexture: new NineSliceTexture2D(
                _texture2DLoader.FromFile(
                    GraphicsDevice,
                    "Content/Textures/surfacegrey.png"),
                new NineSliceTextureOptions
                {
                    TopMargin = 4,
                    LeftMargin = 4,
                    BottomMargin = 4,
                    RightMargin = 4
                }),
            sliderBarTint: Color.DarkGray,
            buttonTexture: new NineSliceTexture2D(
                _texture2DLoader.FromFile(
                    GraphicsDevice,
                    "Content/Textures/surfacegrey.png"),
                new NineSliceTextureOptions
                {
                    TopMargin = 4,
                    LeftMargin = 4,
                    BottomMargin = 4,
                    RightMargin = 4
                }),
            buttonTint: Color.Red,
            buttonHoverTint: Color.LightPink);              

        _tabbedContainer.TabPages.Add(new TabPage("Hello World!!!", [_testSliderBar ]));

        _orangeButton = new RetroSpriteNineSliceButton(
            "OrangeButton",
            "Orange",
            new Point(8, 8),
            new Point(200, 50),
            false,
            foregroundColor: Color.White,
            upTint: Color.Orange,
            downTint: Color.Orange,
            upTexture: new NineSliceTexture2D(
                _texture2DLoader.FromFile(
                    GraphicsDevice,
                    "Content/Textures/greybuttonup.png"),
                new NineSliceTextureOptions
                {
                    TopMargin = 4,
                    LeftMargin = 4,
                    BottomMargin = 8,
                    RightMargin = 4
                }),
            downTexture: new NineSliceTexture2D(
                _texture2DLoader.FromFile(
                    GraphicsDevice,
                    "Content/Textures/greybuttondown.png"),
                new NineSliceTextureOptions
                {
                    TopMargin = 6,
                    LeftMargin = 4,
                    BottomMargin = 6,
                    RightMargin = 4
                }),
            font: _font);

        _tabbedContainer.TabPages.Add(new TabPage("Oranges", [ _orangeButton ]));

        _testContainer.Children.Add(_testButton);
        _testContainer.Children.Add(_testProgressBarLabel);
        _testContainer.Children.Add(_testProgressBar);
        _testContainer.Children.Add(_testCheckBox);

        _exitButton = new RetroSpriteNineSliceButton(
            "ExitButton",
            "Exit",
            new Point(Graphics.PreferredBackBufferWidth - 208, 8),
            new Point(200, 50),
            false,
            foregroundColor: Color.White,
            upTint: Color.Green,
            downTint: Color.Green,
            upTexture: new NineSliceTexture2D(
                _texture2DLoader.FromFile(
                    GraphicsDevice,
                    "Content/Textures/greybuttonup.png"),
                new NineSliceTextureOptions
                {
                    TopMargin = 4,
                    LeftMargin = 4,
                    BottomMargin = 8,
                    RightMargin = 4
                }),
            downTexture: new NineSliceTexture2D(
                _texture2DLoader.FromFile(
                    GraphicsDevice,
                    "Content/Textures/greybuttondown.png"),
                new NineSliceTextureOptions
                {
                    TopMargin = 6,
                    LeftMargin = 4,
                    BottomMargin = 6,
                    RightMargin = 4
                }),
            font: _font);
        _exitButton.Clicked += ExitButton_Clicked;

        _radialGradientTexture = new RadialRetroGradientTexture2D(
            new RadialRetroGradientOptions
            {
                CentrePoint = new SixLabors.ImageSharp.PointF(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height - 80),
                Radius = GraphicsDevice.Viewport.Width,
                FromColor = Color.Yellow,
                ToColor = Color.Purple,
                GradientStops = 8
            });

        _parallaxScroller = new MiniParallaxScroller(
            _texture2DLoader,
            new MiniParallaxScrollerOptions
        {
            Layers = new List<MiniParallaxScrollerLayer>
            {
                new ("Content/Textures/grass.png", 6.0f, 0),
                new ("Content/Textures/road.png", 4.0f, 30),
                new ("Content/Textures/sandyrocks.png", 2.0f, 60),
            },
            ViewportWidth = GraphicsDevice.Viewport.Width,
            ViewportHeight = GraphicsDevice.Viewport.Height
        });

        _previousMouseState = Mouse.GetState();
    }

    private void ExitButton_Clicked(object? sender, EventArgs e)
    {
        this.Exit();
    }

    private void TestButton_Clicking(object? sender, EventArgs e)
    {
        if (sender is RetroSpriteNineSliceButton button)
        {
            button.Text = "Clicking";
        }
    }

    private void TestButton_Clicked(object? sender, EventArgs e)
    {
        if (sender is RetroSpriteNineSliceButton button)
        {
            button.Text = "Clicked!";
            _testProgressBar!.Value = (float)_rnd.NextDouble();
        }
    }

    protected override void OnUpdate(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        _parallaxScroller?.Update();

        var currentMouseState = Mouse.GetState();

        _tabbedContainer?.Update(
            currentMouseState,
            _previousMouseState);

        _exitButton?.Update(
            currentMouseState,
            _previousMouseState);

        _previousMouseState = currentMouseState;
    }

    protected override void OnDraw(
        GameTime gameTime,
        SpriteBatch spriteBatch)
    {
        _radialGradientTexture?.Draw(
            GraphicsDevice.Viewport.Width,
            GraphicsDevice.Viewport.Height, 
            spriteBatch,
            new Rectangle(
                0,
                0,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height));

        _tabbedContainer?.Draw(spriteBatch);
        _exitButton?.Draw(spriteBatch);
        _parallaxScroller?.Draw(spriteBatch);
        spriteBatch?.DrawString(_font, $"FPS: {Fps}", new Vector2(10, 10), Color.White);
    }

    protected override void OnUnloadContent()
    {
        _testButton?.Dispose();
        _exitButton?.Dispose();
        _radialGradientTexture?.Dispose();
        _parallaxScroller?.Dispose();
    }
}
