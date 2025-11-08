using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroLibrary;
using System;
using System.Collections.Generic;

namespace RetroDemo
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private RetroSpriteSmartButton _testButton;

        private MiniParallaxScroller _parallaxScroller;
        private RadialRetroGradientTexture2D _radialGradientTexture;
        //private MultiSlicePieTexture2D _godRaysTexture;
        private MouseState _previousMouseState;

        //private float _godRaysAngle = 0f;
        private double _frameCounter;
        private double _elapsedTime;
        private int _fps;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            var display = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            _graphics.PreferredBackBufferWidth = 800; // display.Width;
            _graphics.PreferredBackBufferHeight = 600; // display.Height;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

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

            _testButton = new RetroSpriteSmartButton(
                "TestButton",
                "Test Button",
                new Point(200, 200),
                new Point(200, 50),
                foregroundColor: Color.White,
                tint: Color.Red,
                upSmartButtonTexture: new SmartButtonTexture2D(
                    Texture2D.FromFile(GraphicsDevice, "Content/Textures/greybuttonup.png"),
                    new SmartButtonOptions
                    {
                        TopMargin = 4,
                        LeftMargin = 4,
                        BottomMargin = 8,
                        RightMargin = 4
                    }),
                downSmartButtonTexture: new SmartButtonTexture2D(
                    Texture2D.FromFile(GraphicsDevice, "Content/Textures/greybuttondown.png"),
                    new SmartButtonOptions
                    {
                        TopMargin = 6,
                        LeftMargin = 4,
                        BottomMargin = 6,
                        RightMargin = 4
                    }),
                font: _font);
            _testButton.Clicked += TestButton_Clicked;
            _testButton.Clicking += TestButton_Clicking;

            _radialGradientTexture = new RadialRetroGradientTexture2D();

            _parallaxScroller = new MiniParallaxScroller(new MiniParallaxScrollerOptions
            {
                Layers = new List<MiniParallaxScrollerLayer>
                {
                    new MiniParallaxScrollerLayer("Content/Textures/grass.png", 6.0f, 0),
                    new MiniParallaxScrollerLayer("Content/Textures/road.png", 4.0f, 30),
                    new MiniParallaxScrollerLayer("Content/Textures/sandyrocks.png", 2.0f, 60),
                },
                ViewportWidth = GraphicsDevice.Viewport.Width,
                ViewportHeight = GraphicsDevice.Viewport.Height
            });

            //_godRaysTexture = new MultiSlicePieTexture2D();

            _previousMouseState = Mouse.GetState();
        }

        private void BlueButton_Clicked(object sender, EventArgs e)
        {
            this.Exit();
        }

        private void TestButton_Clicking(object sender, EventArgs e)
        {
            var button = sender as RetroSpriteSmartButton;

            if (button != null)
            {
                button.Text = "Clicking";
            }
        }

        private void TestButton_Clicked(object? sender, EventArgs e)
        {
            var button = sender as RetroSpriteSmartButton;

            if (button != null)
            {
                button.Text = "Clicked!";
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            _parallaxScroller.Update();

            var currentMouseState = Mouse.GetState();

            _testButton?.Update(
                currentMouseState,
                _previousMouseState);

            _previousMouseState = currentMouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
            _frameCounter++;

            if (_elapsedTime >= 1.0)
            {
                _fps = (int)_frameCounter;
                _frameCounter = 0;
                _elapsedTime = 0;
            }

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _radialGradientTexture.Draw(
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
                _spriteBatch,
                new Rectangle(
                    0,
                    0,
                    GraphicsDevice.Viewport.Width,
                    GraphicsDevice.Viewport.Height));

            ////_godRaysAngle += 0.1f;
            ////if (_godRaysAngle > 360f)
            ////{
            ////    _godRaysAngle = 360f - _godRaysAngle;
            ////}
            ////_godRaysTexture.Draw(
            ////    new MultiSlicePieOptions
            ////    {
            ////        X = GraphicsDevice.Viewport.Width / 2,
            ////        Y = GraphicsDevice.Viewport.Height - 80,
            ////        Width = GraphicsDevice.Viewport.Width,
            ////        Height = GraphicsDevice.Viewport.Height,
            ////        Radius = GraphicsDevice.Viewport.Width,
            ////        Angle = _godRaysAngle,
            ////        Color = SixLabors.ImageSharp.Color.Gold.WithAlpha(0.1f),
            ////        NumSlices = 12
            ////    },
            ////    _spriteBatch);

            _testButton.Draw(_spriteBatch);

            _parallaxScroller.Draw(_spriteBatch);

            _spriteBatch.DrawString(_font, $"FPS: {_fps}", new Vector2(10, 10), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            _testButton.Dispose();
            _radialGradientTexture.Dispose();
            _parallaxScroller.Dispose();
            base.UnloadContent();
        }
    }
}
