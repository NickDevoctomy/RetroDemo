using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroLibrary;

namespace RetroDemo
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly Texture2DResourceLoader _texture2DLoader = null!;
        private SpriteBatch? _spriteBatch;
        private SpriteFont? _font;

        private RetroSpriteContainer? _testContainer;
        private RetroSpriteNineSliceButton? _testButton;
        private RetroSpriteLabel? _testProgressBarLabel;
        private RetroSpriteProgressBar? _testProgressBar;
        private RetroSpriteCheckBox? _testCheckBox;

        private RetroSpriteTabbedContainer _tabbedContainer;

        private RetroSpriteNineSliceButton? _orangeButton;
        private RetroSpriteNineSliceButton? _exitButton;

        private MiniParallaxScroller? _parallaxScroller;
        private RadialRetroGradientTexture2D? _radialGradientTexture;
        private MouseState _previousMouseState;

        private double _frameCounter;
        private double _elapsedTime;
        private int _fps;
        private readonly Random _rnd = new (Environment.TickCount);

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            var display = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            _graphics.PreferredBackBufferWidth = 800; // display.Width;
            _graphics.PreferredBackBufferHeight = 600; // display.Height;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            _texture2DLoader= new Texture2DResourceLoader(GraphicsDevice);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load a default font (you'll need to add a font to your Content pipeline)
            // For now, we'll create a simple button after the font is loaded
            try
            {
                _font = Content.Load<SpriteFont>("DefaultFont");
            }
            catch
            {
                // If no font is available, we'll need to handle this gracefully
                // For now, let's create the button anyway and it will handle null font
                _font = null;
            }

            _testContainer = new RetroSpriteGroupContainer(
                "TestGroupContainer",
                "Some really long text in here!",
                new Point(8, 8),
                new Point(380, 250),
                borderTexture: new NineSliceTexture2D(
                    _texture2DLoader.FromFile("Content/Textures/surface.png"),
                    new NineSliceTextureOptions
                    {
                        TopMargin = 4,
                        LeftMargin = 4,
                        BottomMargin = 4,
                        RightMargin = 4
                    }),
                borderOuterTopMargin: 8,
                groupLabelTexture: new NineSliceTexture2D(
                    _texture2DLoader.FromFile("Content/Textures/surface.png"),
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
                    _texture2DLoader.FromFile("Content/Textures/greybuttonup.png"),
                    new NineSliceTextureOptions
                    {
                        TopMargin = 4,
                        LeftMargin = 4,
                        BottomMargin = 8,
                        RightMargin = 4
                    }),
                downTexture: new NineSliceTexture2D(
                    _texture2DLoader.FromFile("Content/Textures/greybuttondown.png"),
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
                    _texture2DLoader.FromFile("Content/Textures/border.png"),
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
                    _texture2DLoader.FromFile("Content/Textures/surface.png"),
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
                    _texture2DLoader.FromFile("Content/Textures/surfacegrey.png"),
                    new NineSliceTextureOptions
                    {
                        TopMargin = 4,
                        LeftMargin = 4,
                        BottomMargin = 4,
                        RightMargin = 4
                    });
            _tabbedContainer.TabPageTint = new Color(Color.LightGray, 0.5f);
            _tabbedContainer.TabUpTexture = new NineSliceTexture2D(
                    _texture2DLoader.FromFile("Content/Textures/tab.png"),
                    new NineSliceTextureOptions
                    {
                        TopMargin = 4,
                        LeftMargin = 4,
                        BottomMargin = 4,
                        RightMargin = 4
                    });
            _tabbedContainer.TabUpTint = new Color(Color.LightGray, 0.1f);
            _tabbedContainer.TabDownTexture = new NineSliceTexture2D(
                    _texture2DLoader.FromFile("Content/Textures/tab.png"),
                    new NineSliceTextureOptions
                    {
                        TopMargin = 4,
                        LeftMargin = 4,
                        BottomMargin = 4,
                        RightMargin = 4
                    });
            _tabbedContainer.TabDownTint = new Color(Color.LightGray, 0.5f);
            _tabbedContainer.TabPages.Add(new TabPage("Apple", [ _testContainer ]));
            _tabbedContainer.TabPages.Add(new TabPage("Hello World!!!", []));

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
                    _texture2DLoader.FromFile("Content/Textures/greybuttonup.png"),
                    new NineSliceTextureOptions
                    {
                        TopMargin = 4,
                        LeftMargin = 4,
                        BottomMargin = 8,
                        RightMargin = 4
                    }),
                downTexture: new NineSliceTexture2D(
                    _texture2DLoader.FromFile("Content/Textures/greybuttondown.png"),
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
                new Point(_graphics.PreferredBackBufferWidth - 208, 8),
                new Point(200, 50),
                false,
                foregroundColor: Color.White,
                upTint: Color.Green,
                downTint: Color.Green,
                upTexture: new NineSliceTexture2D(
                    _texture2DLoader.FromFile("Content/Textures/greybuttonup.png"),
                    new NineSliceTextureOptions
                    {
                        TopMargin = 4,
                        LeftMargin = 4,
                        BottomMargin = 8,
                        RightMargin = 4
                    }),
                downTexture: new NineSliceTexture2D(
                    _texture2DLoader.FromFile("Content/Textures/greybuttondown.png"),
                    new NineSliceTextureOptions
                    {
                        TopMargin = 6,
                        LeftMargin = 4,
                        BottomMargin = 6,
                        RightMargin = 4
                    }),
                font: _font);
            _exitButton.Clicked += ExitButton_Clicked;

            _radialGradientTexture = new RadialRetroGradientTexture2D();

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

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            _parallaxScroller?.Update();

            var currentMouseState = Mouse.GetState();

            _tabbedContainer.Update(
                currentMouseState,
                _previousMouseState);

            _exitButton?.Update(
                currentMouseState,
                _previousMouseState);

            _previousMouseState = currentMouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
            _frameCounter++;

            if (_elapsedTime >= 1.0)
            {
                _fps = (int)_frameCounter;
                _frameCounter = 0;
                _elapsedTime = 0;
            }

            _spriteBatch?.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            _radialGradientTexture?.Draw(
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                new RadialRetroGradientOptions
                {
                    CentrePoint = new SixLabors.ImageSharp.PointF(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height - 80),
                    Radius = GraphicsDevice.Viewport.Width,
                    FromColor = Color.Yellow,
                    ToColor = Color.Purple,
                    GradientStops = 8
                },
                _spriteBatch!,
                new Rectangle(
                    0,
                    0,
                    GraphicsDevice.Viewport.Width,
                    GraphicsDevice.Viewport.Height));

            //_testContainer?.Draw(_spriteBatch!);

            _tabbedContainer?.Draw(_spriteBatch!);

            _exitButton?.Draw(_spriteBatch!);

            _parallaxScroller?.Draw(_spriteBatch!);

            _spriteBatch?.DrawString(_font, $"FPS: {_fps}", new Vector2(10, 10), Color.White);

            _spriteBatch?.End();
        }

        protected override void UnloadContent()
        {
            _testButton?.Dispose();
            _exitButton?.Dispose();
            _radialGradientTexture?.Dispose();
            _parallaxScroller?.Dispose();
            base.UnloadContent();
        }
    }
}
