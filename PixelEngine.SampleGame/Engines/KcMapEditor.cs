public class KcMapEditor : Engine
{
    private string _renderStateFile, _planeFile;
    private StreamScroller? _streamScroller;
    private MovingSprite _cameraFocus;
    private CoordinateTranslator _coordinateTranslator;
    private LevelTileMap _map;
    private TileProperties _tileProperties;

    public CoordinateTranslator CoordinateTranslator => _coordinateTranslator;
    
    public KcMapEditor(string renderStateFile, string planeFile, string tileProperiesFile, RenderService renderService, InputManager inputManager, Specs specs)
        : base(renderService, inputManager, specs)
    {
        _tileProperties = new TileProperties(tileProperiesFile);
        _renderStateFile = renderStateFile;
        _planeFile = planeFile;
    }

    public TileType TilePropertiesAt(int tileX, int tileY) =>
        _tileProperties[_map.Tiles[tileX, tileY].Index];

    public override void Load()
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

        var camera = new Camera();
        _coordinateTranslator = new CoordinateTranslator(_renderService.Layers.Foreground, _map, camera, _specs);
        _cameraFocus = new MovingSprite(_renderService.Sprites[0], _coordinateTranslator, _specs);
        _streamScroller = new StreamScroller(
            _renderService.Layers.Background,
            _renderService.Layers.Foreground,
            _map,
            _cameraFocus,
            _coordinateTranslator,
            camera,
            Array.Empty<BackgroundScrollLayer>(),
            Array.Empty<BackgroundScrollLayer>(),
            _specs);

        _cameraFocus.WorldX = 160;
        _cameraFocus.WorldY = 128;

        _cameraFocus.Update();
        _streamScroller!.Update();
        _streamScroller.RefreshEntireScreen();
    }

    public override void Update(ulong frameNumber)
    {
        var scrollSpeed = _inputManager.Player1.KeyDown(GamepadButtons.A) ? 4 : 1;

        if (_inputManager.Player1.KeyDown(GamepadButtons.Right))
            _cameraFocus.HorizontalMotion.SetSpeedImmediate(scrollSpeed);
        else if (_inputManager.Player1.KeyDown(GamepadButtons.Left))
            _cameraFocus.HorizontalMotion.SetSpeedImmediate(-scrollSpeed);
        else
            _cameraFocus.HorizontalMotion.SetSpeedImmediate(0);

        if (_inputManager.Player1.KeyDown(GamepadButtons.Down))
            _cameraFocus.VerticalMotion.SetSpeedImmediate(scrollSpeed);
        else if (_inputManager.Player1.KeyDown(GamepadButtons.Up))
            _cameraFocus.VerticalMotion.SetSpeedImmediate(-scrollSpeed);
        else
            _cameraFocus.VerticalMotion.SetSpeedImmediate(0);

        _cameraFocus.Update();
        _streamScroller!.Update();

        var tileUnderMouse = TileUnderMouse();
        if (_inputManager.Player1.KeyPressed(GamepadButtons.A))
        {
            _tileProperties[tileUnderMouse.Index] = _tileProperties[tileUnderMouse.Index] + 1;
        }
        if (_inputManager.Player1.KeyPressed(GamepadButtons.B))
        {
            _tileProperties[tileUnderMouse.Index] = _tileProperties[tileUnderMouse.Index] - 1;
        }
        if (_inputManager.Player1.KeyPressed(GamepadButtons.C))
        {
            _tileProperties.WriteToDisk();
        }

        Debug.Text1 = $"MOUSE={_inputManager.MousePosition} TILE={tileUnderMouse}";
        Debug.Text2 = _tileProperties[tileUnderMouse.Index].ToString();
    }

    private Tile TileUnderMouse()
    {
        var tile = _coordinateTranslator.ScreenToWorld(_inputManager.MousePosition) / _specs.TileSize;

        return _map.Tiles[tile];
    }
}


