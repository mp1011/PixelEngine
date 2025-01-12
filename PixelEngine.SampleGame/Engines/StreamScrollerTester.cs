public class StreamScrollerTester : Engine
{
    public enum Mode
    {
        Stream,
        Direct,
        FullRefresh,
    }

    private Mode _mode = Mode.Stream;
    private string _renderStateFile, _planeFile;
    private StreamScroller? _streamScroller;
    private MovingSprite _cameraFocus;
    private CoordinateTranslator _coordinateTranslator;
    private Point _fullRefreshOffset = new Point(0, 0);
    private bool _needFullRefresh = false;

    public StreamScrollerTester(string renderStateFile, string planeFile, RenderService renderService, InputManager inputManager, Specs specs)
        : base(renderService, inputManager, specs)
    {
        _renderStateFile = renderStateFile;
        _planeFile = planeFile;        
    }

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

        LevelTileMap map = new LevelTileMap(plane.Skip(4).ToArray(), new Size(width, height), _specs);

        var camera = new Camera();
        _coordinateTranslator = new CoordinateTranslator(_renderService.Layers.Foreground, map, camera, _specs);
        _cameraFocus = new MovingSprite(_renderService.Sprites[0], _coordinateTranslator, _specs);
        _streamScroller = new StreamScroller(
            _renderService.Layers.Background,
            _renderService.Layers.Foreground,
            map,
            _cameraFocus,
            _coordinateTranslator,
            camera,
            Array.Empty<BackgroundScrollLayer>(),
            Array.Empty<BackgroundScrollLayer>(),
            _specs);

        _cameraFocus.WorldX = 160;
        _cameraFocus.WorldY = 128;
        _needFullRefresh = true;
    }

    public override void Update(ulong frameNumber)
    {
        var scrollSpeed = _inputManager.Player1.KeyDown(GamepadButtons.A) ? 10 : 1;

        if (_inputManager.Player1.KeyPressed(GamepadButtons.B))
        {
            _needFullRefresh = true;
            _mode++;
            if (_mode == (Mode)3)
                _mode = Mode.Stream;
        }   

        var hScroll = _renderService.Layers.Foreground.HScrollTable.Values[0];
        var vScroll = _renderService.Layers.Foreground.VScrollTable.Values[0];

        if (_mode == Mode.Stream)
        {
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

            if(_needFullRefresh)
            {
                _needFullRefresh = false;
                _streamScroller.RefreshEntireScreen();
            }

            Debug.Text1 = $"MODE=Stream {_streamScroller}";
        }
        else if(_mode == Mode.Direct)
        {
            if (_inputManager.Player1.KeyDown(GamepadButtons.Right))
                _renderService.Layers.Foreground.HScrollTable.AddAll((short)-scrollSpeed);
            if (_inputManager.Player1.KeyDown(GamepadButtons.Left))
                _renderService.Layers.Foreground.HScrollTable.AddAll((short)scrollSpeed);
            if (_inputManager.Player1.KeyDown(GamepadButtons.Up))
                _renderService.Layers.Foreground.VScrollTable.AddAll((short)-scrollSpeed);
            if (_inputManager.Player1.KeyDown(GamepadButtons.Down))
                _renderService.Layers.Foreground.VScrollTable.AddAll((short)scrollSpeed);

            var cornerPlanePoint = _coordinateTranslator.ScreenToPlane(new Point(0, 0));

            Debug.Text1 = $"MODE=Direct    Scroll={hScroll},{vScroll}    CPP={cornerPlanePoint}  ";
        }
        else if (_mode == Mode.FullRefresh)
        {
            if (_inputManager.Player1.KeyPressed(GamepadButtons.Right))
                _fullRefreshOffset = _fullRefreshOffset.Add(1, 0);
            if (_inputManager.Player1.KeyPressed(GamepadButtons.Left))
                _fullRefreshOffset = _fullRefreshOffset.Add(-1, 0);
            if (_inputManager.Player1.KeyPressed(GamepadButtons.Up))
                _fullRefreshOffset = _fullRefreshOffset.Add(0, -1);
            if (_inputManager.Player1.KeyPressed(GamepadButtons.Down))
                _fullRefreshOffset = _fullRefreshOffset.Add(0, 1);

            _streamScroller!.RefreshEntireScreen(_fullRefreshOffset);
            Debug.Text1 = $"MODE=FullRefresh   Offset={_fullRefreshOffset}";
        }

    }
}


