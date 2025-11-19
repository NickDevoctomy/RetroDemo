using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroLibrary.Core.Interfaces;

namespace RetroLibrary.Core.Base;

public class RetroGameBase : Game
{
    private readonly RetroGameContext _retroGameContext;
    private double _frameCounter;
    private double _elapsedTime;
    private int _fps;
    private SpriteBatch? _spriteBatch;
    private MouseState _previousMouseState;

    public RetroGameBase(RetroGameContext retroGameContext)
    {
        _retroGameContext = retroGameContext;
        _retroGameContext.Initialse(this);
        Graphics = _retroGameContext.GraphicsDeviceManager!;
        IsMouseVisible = true;
    }

    public int Fps => _fps;

    public GraphicsDeviceManager Graphics { get; private set; }

    protected virtual void OnLoadContent()
    {
        _previousMouseState = Mouse.GetState();
    }

    protected virtual void OnUpdate(
        GameTime gameTime,
        MouseState currentState,
        MouseState previousState)
    {
        // !!! Causing flicker for some reason or another
        foreach (var sprite in _retroGameContext.RetroGameLoaderService.Sprites)
        {
            sprite.Update(
                currentState,
                previousState);
        }
    }

    protected virtual void OnDraw(
        GameTime gameTime,
        SpriteBatch spriteBatch)
    {
        foreach (var sprite in _retroGameContext.RetroGameLoaderService.Sprites)
        {
            sprite.Draw(spriteBatch);
        }
    }

    protected virtual void OnUnloadContent()
    {
        // Do nothing by default
    }

    protected override void LoadContent()
    {
        _retroGameContext.LoadGameDefinition();
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        OnLoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        var currentMouseState = Mouse.GetState();

        OnUpdate(
            gameTime,
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

        GraphicsDevice.Clear(_retroGameContext.RetroGameLoaderService.BackgroundColor);

        _spriteBatch!.Begin(
            SpriteSortMode.Immediate,
            BlendState.AlphaBlend,
            null,
            null,
            new RasterizerState
            {
                ScissorTestEnable = true
            });

        OnDraw(
            gameTime,
            _spriteBatch!);

        _spriteBatch?.DrawString(
            _retroGameContext.ResourceManager.GetResource<SpriteFont>("DefaultFont"),
            $"FPS: {_fps}",
            new Vector2(10, 10),
            Color.White);

        _spriteBatch!.End();
    }

    protected override void UnloadContent()
    {
        _spriteBatch?.Dispose();
        _spriteBatch = null;

        OnUnloadContent();
        base.UnloadContent();
    }
}
