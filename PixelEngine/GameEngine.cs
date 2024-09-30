
using PixelEngine.Models.Graphics.GensLike;
using PixelEngine.Services;

namespace PixelEngine;

public class GameEngine : Game
{
    private readonly Specs _specs = new();

    private DiskResourceLoader _diskResourceLoader = new DiskResourceLoader();
    private IRenderStrategy _renderStrategy;
    private RenderService _renderService;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private RenderTarget2D _renderTarget;
    private FrameRateDisplay _frameRateDisplay;

    public GameEngine()
    {
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
        var patternTable = new PatternTable(_diskResourceLoader.Load("SampleVRAM/kc.ram"), _specs);

        _renderService = new RenderService(_specs, patternTable, new LayerGroup(_specs));
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _renderTarget = new RenderTarget2D(GraphicsDevice, _specs.ScreenWidth, _specs.ScreenHeight);

        _renderStrategy = new PrimitivesRenderStrategy(_renderService);

        _renderStrategy.Initialize(GraphicsDevice, _specs);
        _frameRateDisplay = new FrameRateDisplay(Content.Load<SpriteFont>("DiagnosticFont"));

        _renderService.Palette = new Palette(new Color[]
        {
            new Color(32,96,224),
            new Color(0,0,0),
            new Color(32,32,0),
            new Color(96,96,64),
            new Color(128,g: 128,96),
            new Color(192,g: 192,160),
            new Color(192,g: 128,96),
            new Color(64,g: 0,0),
            new Color(0,g: 96,0),
            new Color(0,g: 160,64),
            new Color(64,g: 224,64),
            new Color(96,g: 32,0),
            new Color(128,g: 64,32),
            new Color(160,g: 96,64),
            new Color(128,g: 64,96),
            new Color(255,g: 255,255),
        });

        var bg = _renderService.LayerGroup.Background;

        bg.Tiles.ForEach((x, y) =>
        {
            if (x < 8 && y < 8)
            {
                bg.Tiles[x, y] = new Tile(192 + x + (y*16), TileFlags.Normal, PaletteIndex.P0);
            }
        });
 


        //_layers[0].PixelData.CopyFrom(vram, new Rectangle(0, 120, 128, 16), new Point(0, 0));
        //_layers[1].PixelData.CopyFrom(vram, new Rectangle(0, 208, 128, 64), new Point(0, 32));
        // _layers[1].RasterInterupts.Add(new TestRasterInterupt());
    }

    protected override void Update(GameTime gameTime)
    {
        _renderService.RefreshFrameColors();
        _renderStrategy.OnFrameUpdate();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
       
        GraphicsDevice.SetRenderTarget(_renderTarget);
        GraphicsDevice.Clear(Color.Black);

        _renderStrategy.Draw(GraphicsDevice, _spriteBatch);
    
        GraphicsDevice.SetRenderTarget(null);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);


        var aspectRatio = (double)_renderTarget.Height / _renderTarget.Width;

        _spriteBatch.Draw(_renderTarget, new Rectangle(
                           x: 0,
                           y: 0,
                           width: Window.ClientBounds.Width,
                           height: (int)(Window.ClientBounds.Width * aspectRatio)), Color.White);

        _spriteBatch.End();

        _frameRateDisplay.Draw(_spriteBatch, Window);

        base.Draw(gameTime);
    }
}