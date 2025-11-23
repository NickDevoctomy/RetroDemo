using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RetroLibrary.Core.Base;

public class RetroGameBase : Game
{
    private double _frameCounter;
    private double _elapsedTime;
    private int _fps;
    private SpriteBatch? _spriteBatch;
    private MouseState _previousMouseState;

    public RetroGameBase(RetroGameContext retroGameContext)
    {
        RetroGameContext = retroGameContext;
        RetroGameContext.Initialse(this);
        Graphics = RetroGameContext.GraphicsDeviceManager!;
        IsMouseVisible = true;
    }

    public int Fps => _fps;

    public GraphicsDeviceManager Graphics { get; private set; }

    public RetroGameContext RetroGameContext { get; init; }

    public void SetTargetFps(int targetFps)
    {
        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / targetFps);
    }

    protected virtual void OnLoadContent()
    {
        _previousMouseState = Mouse.GetState();
    }

    protected virtual void OnUpdate(
        GameTime gameTime,
        MouseState currentState,
        MouseState previousState)
    {
        foreach (var sprite in RetroGameContext.RetroGameLoaderService.Sprites)
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
        var timings = new Dictionary<string, TimeSpan>();
        foreach (var sprite in RetroGameContext.RetroGameLoaderService.Sprites)
        {
            var startedAt = DateTime.Now;
            sprite.Draw(spriteBatch);
            var elapsed = DateTime.Now - startedAt;
            timings.Add(sprite.Name, elapsed);
        }

        var slowest = timings.OrderByDescending(t => t.Value).FirstOrDefault();
        System.Diagnostics.Debug.WriteLine($"Slowest sprite = {slowest.Key} - {slowest.Value}");
    }

    protected virtual void OnUnloadContent()
    {
        // Do nothing by default
    }

    protected override void LoadContent()
    {
        RetroGameContext.LoadGameDefinition();
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

        GraphicsDevice.Clear(RetroGameContext.RetroGameLoaderService.BackgroundColor);

        _spriteBatch!.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            new SamplerState
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                Filter = TextureFilter.Point
            },
            null,
            new RasterizerState
            {
                ScissorTestEnable = true
            });

        OnDraw(
            gameTime,
            _spriteBatch!);

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