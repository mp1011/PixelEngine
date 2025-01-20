public class KcMockup : Engine
{
    private string _renderStateFile, _planeFile;
    private LevelTileMap _map;
    private TileProperties _tileProperties;


    private TileAnimator? _frameAnimator;
    private TileFont? _hudFont;
    private SpriteString? _clockString;
    private Clock? _clock;
    private WaterWaver? _waterWaver;
    private Camera? _camera;
    private CoordinateTranslator? _coordinateTranslator;
    private StreamScroller? _streamScroller;
    private PlayerController _playerController;

    public KcMockup(string renderStateFile, string planeFile, string tilePropertiesFile, RenderService renderService, InputManager inputManager, Specs specs) : base(renderService, inputManager, specs)
    {
        _renderStateFile = renderStateFile;
        _planeFile = planeFile;
        _tileProperties = new TileProperties(tilePropertiesFile);
    }

    public override void Load()
    {
        LoadVram();

        _frameAnimator = new TileAnimator(_renderService);
        _frameAnimator.AddAnimation(Workbench.ExtractTileAnimation("gem", 0x6ea, 0x6fd, [6, 6, 6]));
        _frameAnimator.AddAnimation(Workbench.ExtractTileAnimation("ankh", 0x231, 0x233, [120, 6, 6, 6]));

        _hudFont = Workbench.CreateHudFont();
        _clockString = Workbench.SetupClockSprites(_hudFont, _renderService);
        _clock = new Clock(2, 55, _clockString);

        _waterWaver = new WaterWaver(_renderService.Layers.Background, 128, 200);

        var playerAnimations = Workbench.LoadPlayerAnimations();
        _frameAnimator.AddAnimation(playerAnimations);

        _clockString.Text = "1:23";

        _camera = new Camera();
        _coordinateTranslator = new CoordinateTranslator(_renderService.Layers.Foreground, _map, _camera, _specs);

        Debug.CoordinateTranslator = _coordinateTranslator;

        var collisionManager = new KcCollisionManager(_map, _coordinateTranslator, _tileProperties, _specs);

        var player = new MovingSprite(_renderService.Sprites[11], _coordinateTranslator, _specs);
        _playerController = new PlayerController(
            _inputManager,
            playerAnimations,
            player,
            collisionManager);

        var bgHScrollLayers = new BackgroundScrollLayer[]
        {
            new BackgroundScrollLayer(56, 0.2),
            new BackgroundScrollLayer(148, 0.4),
        };

        var bgVScrollLayers = new BackgroundScrollLayer[]
        {
            new BackgroundScrollLayer(1, 0.5),
        };

        _streamScroller = new StreamScroller(_renderService.Layers.Background, _renderService.Layers.Foreground, _map, player,
            _coordinateTranslator, _camera, bgHScrollLayers, bgVScrollLayers, _specs);


        player.WorldX = 32;
        player.WorldY = 32;

        player.Update();
        _streamScroller!.Update();
        _streamScroller.RefreshEntireScreen();
    }

    private void LoadVram()
    {
        var renderState = DiskResourceLoader.Load($"RenderStates\\{_renderStateFile}");
        var plane = DiskResourceLoader.Load($"Nametables\\{_planeFile}");

        byte[] registers = new byte[20];
        byte[] vram = new byte[0x10000];
        byte[] cram = new byte[256];
        byte[] vsram = new byte[256];

        Array.Copy(renderState, 0, registers, 0, registers.Length);
        Array.Copy(renderState, registers.Length, vram, 0, vram.Length);
        Array.Copy(renderState, registers.Length + vram.Length, cram, 0, cram.Length);
        Array.Copy(renderState, registers.Length + vram.Length + cram.Length, vsram, 0, vsram.Length);

        _renderService.SetAllData(
            GensSaveStateImporter.ReadRegisters(registers, 0),
            vram,
            cram,
            vsram);

        int width = plane[0] << 8 | plane[1];
        int height = plane[2] << 8 | plane[3];

        _map = new LevelTileMap(plane.Skip(4).ToArray(), new Size(width, height), _specs);
    }

    public override void Update(ulong frameNumber)
    {
        _frameAnimator!.Update(frameNumber);
        _clock!.Update(frameNumber);
        _streamScroller!.Update();
        _playerController!.Update();
        _waterWaver.Update();

        var tileUnderMouse = TileUnderMouse();
        Debug.Text1 = $"T={tileUnderMouse.Index} {_tileProperties[tileUnderMouse.Index]}";
    }

    private Tile TileUnderMouse()
    {
        var tile = _coordinateTranslator.ScreenToWorld(_inputManager.MousePosition) / _specs.TileSize;

        return _map.Tiles[tile];
    }
}