
public class XnaGameEngine : Game
{
    private readonly GameEngine _engine;
    private IRenderStrategy _renderStrategy;
    private FrameRateDisplay _frameRateDisplay;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private RenderTarget2D _renderTarget;

    private Specs Specs => _engine.Specs;
    
    private RenderService RenderService => _engine.RenderService;

    public XnaGameEngine(GameEngine gameEngine)
    {
        _engine = gameEngine;
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;

        IsFixedTimeStep = false;
      //  IsFixedTimeStep = true;
      //  TargetElapsedTime = TimeSpan.FromMilliseconds(16.7);
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();

    }

    protected override void LoadContent()
    { 
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _renderTarget = new RenderTarget2D(GraphicsDevice, Specs.ScreenWidth, Specs.ScreenHeight);

        _renderStrategy = new PrimitivesRenderStrategy(RenderService as XnaRenderService);

        _renderStrategy.Initialize(GraphicsDevice, Specs);
        _frameRateDisplay = new FrameRateDisplay(Content.Load<SpriteFont>("DiagnosticFont"));

        var bg = RenderService.LayerGroup.Background;

        bg.Tiles.ForEach((x, y) =>
        {
            if (x < 8 && y < 8)
            {
                bg.Tiles[x, y] = new Tile(x,y + 64, TileFlags.Normal, PaletteIndex.P0);
            }

            if(x < 4 & y > 16 && y < 20)
            {
                bg.Tiles[x, y] = new Tile(0,y+80, TileFlags.FlipX, PaletteIndex.P0);
            }
        });
 
        var fg = RenderService.LayerGroup.Foreground;
        fg.Tiles.ForEach((x, y) =>
        {
            if (x < 8 && y < 8)
            {
                fg.Tiles[x, y] = new Tile(x, y + 64, TileFlags.Normal, PaletteIndex.P0);
            }
        });

        //_layers[0].PixelData.CopyFrom(vram, new Rectangle(0, 120, 128, 16), new Point(0, 0));
        //_layers[1].PixelData.CopyFrom(vram, new Rectangle(0, 208, 128, 64), new Point(0, 32));
        // _layers[1].RasterInterupts.Add(new TestRasterInterupt());
    }

    protected override void Update(GameTime gameTime)
    {
        RenderService.RefreshFrameColors();
        _renderStrategy.OnFrameUpdate();

        var keys = Keyboard.GetState();
        var bg = RenderService.LayerGroup.Background;
        bg.Scroll.X--;
     //   bg.Scroll.Y--;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_renderTarget);
        GraphicsDevice.Clear(XnaColor.Black);

        _renderStrategy.Draw(GraphicsDevice, _spriteBatch);

        GraphicsDevice.SetRenderTarget(null);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);


        var aspectRatio = (double)_renderTarget.Height / _renderTarget.Width;

        _spriteBatch.Draw(_renderTarget, new XnaRectangle(
                           x: 0,
                           y: 0,
                           width: Window.ClientBounds.Width,
                           height: (int)(Window.ClientBounds.Width * aspectRatio)), XnaColor.White);

        _spriteBatch.End();

        _frameRateDisplay.Draw(_spriteBatch, Window);

        base.Draw(gameTime);
    }
}