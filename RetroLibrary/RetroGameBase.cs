using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RetroLibrary;

public class RetroGameBase : Game
{
    private double _frameCounter;
    private double _elapsedTime;
    private int _fps;
    private SpriteBatch? _spriteBatch;

    public RetroGameBase(
        int width,
        int height,
        bool fullscreen)
    {
        Graphics = new GraphicsDeviceManager(this);
        Graphics.PreferredBackBufferWidth = width;
        Graphics.PreferredBackBufferHeight = height;
        Graphics.IsFullScreen = fullscreen;
        Graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    public int Fps => _fps;

    public GraphicsDeviceManager Graphics { get; init; }

    protected virtual void OnLoadContent()
    {
        // Do nothing by default
    }

    protected virtual void OnUpdate(GameTime gameTime)
    {
        // Do nothing by default
    }

    protected virtual void OnDraw(
        GameTime gameTime,
        SpriteBatch spriteBatch)
    {
        // Do nothing by default
    }

    protected virtual void OnUnloadContent()
    {
        // Do nothing by default
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        OnLoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        OnUpdate(gameTime);
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

        _spriteBatch!.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend);

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
