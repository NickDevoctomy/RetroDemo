using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroLibrary;
using SixLabors.ImageSharp;

namespace Project2
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private Button _button;
        private ParallaxScroller _parallaxScroller;
        private RadialRetroGradientTexture2D _radialGradientTexture;
        private MouseState _previousMouseState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
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

            // Create the button after GraphicsDevice and font are available
            _button = new Button(
                GraphicsDevice,
                _font,
                "Click Me!",
                new Vector2(100, 100),
                new Vector2(200, 50))
            {
                BackgroundColor = Microsoft.Xna.Framework.Color.Red,
                ForegroundColor = Microsoft.Xna.Framework.Color.White
            };
            _button.Clicked += Button_Clicked;
            _button.Clicking += Button_Clicking;

            //_linearGradientTexture = new LinearRetroGradientTexture2D(GraphicsDevice);
            _radialGradientTexture = new RadialRetroGradientTexture2D();

            _parallaxScroller = new ParallaxScroller(new ParallaxScrollerOptions
            {
                Layers = new List<ParallaxScrollerLayer>
                {
                    new ParallaxScrollerLayer("Content/Textures/grass1.png", 3.0f, 0),
                    new ParallaxScrollerLayer("Content/Textures/grass2.png", 2.0f, 40),
                    new ParallaxScrollerLayer("Content/Textures/grass3.png", 1.0f, 80),
                },
                ViewportWidth = GraphicsDevice.Viewport.Width,
                ViewportHeight = GraphicsDevice.Viewport.Height
            });

            _previousMouseState = Mouse.GetState();
        }

        private void Button_Clicking(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button != null)
            {
                button.Text = "Clicking";
            }
        }

        private void Button_Clicked(object? sender, EventArgs e)
        {
            var button = sender as Button;

            if (button != null)
            {
                button.Text = "Clicked!";
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _parallaxScroller.Update();

            // Update button with mouse input
            var currentMouseState = Mouse.GetState();
            _button?.Update(
                currentMouseState,
                _previousMouseState);
            _previousMouseState = currentMouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);

            _spriteBatch.Begin();

            ////_linearGradientTexture.Draw(
            ////    GraphicsDevice.Viewport.Width,
            ////    GraphicsDevice.Viewport.Height,
            ////    new LinearRetroGradientOptions
            ////    {
            ////        FromPoint = new Point(0, 0),
            ////        ToPoint = new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
            ////        FromColor = Color.Blue,
            ////        ToColor = Color.Purple,
            ////        GradientStops = 8
            ////    },
            ////    _spriteBatch,
            ////    new Rectangle(
            ////        0,
            ////        0,
            ////        GraphicsDevice.Viewport.Width,
            ////        GraphicsDevice.Viewport.Height));
            ///
            _radialGradientTexture.Draw(
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                new RadialRetroGradientOptions
                {
                    CentrePoint = new PointF(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height - 80),
                    Radius = 700f,
                    FromColor = Microsoft.Xna.Framework.Color.Yellow,
                    ToColor = Microsoft.Xna.Framework.Color.Purple,
                    GradientStops = 8
                },
                _spriteBatch,
                new Microsoft.Xna.Framework.Rectangle(
                    0,
                    0,
                    GraphicsDevice.Viewport.Width,
                    GraphicsDevice.Viewport.Height));

            _button?.Draw(_spriteBatch);

            _parallaxScroller.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            _button?.Dispose();
            _radialGradientTexture.Dispose();
            _parallaxScroller.Dispose();
            base.UnloadContent();
        }
    }
}
