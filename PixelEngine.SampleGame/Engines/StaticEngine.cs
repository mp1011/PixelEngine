public class StaticEngine : Engine
{
    private readonly string _saveStateFilename;
    private readonly string _cramFilename; //savestate doesn't include all color data
    public StaticEngine(string saveStateFilename, string cramFilename, RenderService renderService, InputManager inputManager, Specs specs) 
        : base(renderService, inputManager, specs)
    {
        _saveStateFilename = saveStateFilename;
        _cramFilename = cramFilename;
    }

    public override void Load()
    {
        GensSaveStateImporter.ReadSaveState(DiskResourceLoader.Load($"GensSaveStates\\{_saveStateFilename}"), _renderService, _specs);
        GensVramImporter.LoadColors(DiskResourceLoader.Load($"SampleVRAM\\{_cramFilename}"), _renderService, _specs);
    }

    public override void Update(ulong frameNumber)
    {
        short fSpeed = 8;
        short bSpeed = 4;

        if(_inputManager.Player1.KeyDown(GamepadButtons.A))
        {
            fSpeed = 1;
            bSpeed = 1;
        }

        if(_inputManager.Player1.KeyDown(GamepadButtons.Up))
        {
            _renderService.Layers.Background.VScrollTable.AddAll((short)-bSpeed);
            _renderService.Layers.Foreground.VScrollTable.AddAll((short)-fSpeed);
        }
        else if (_inputManager.Player1.KeyDown(GamepadButtons.Down))
        {
            _renderService.Layers.Background.VScrollTable.AddAll(bSpeed);
            _renderService.Layers.Foreground.VScrollTable.AddAll(fSpeed);
        }

        if (_inputManager.Player1.KeyDown(GamepadButtons.Left))
        {
            _renderService.Layers.Background.HScrollTable.AddAll((short)-bSpeed);
            _renderService.Layers.Foreground.HScrollTable.AddAll((short)-fSpeed);
        }
        else if (_inputManager.Player1.KeyDown(GamepadButtons.Right))
        {
            _renderService.Layers.Background.HScrollTable.AddAll(bSpeed);
            _renderService.Layers.Foreground.HScrollTable.AddAll(fSpeed);
        }

        Debug.Text1 = $"FG H={_renderService.Layers.Foreground.HScrollTable.Values[0]} V={_renderService.Layers.Foreground.VScrollTable.Values[0]}";
        Debug.Text2 = $"BG H={_renderService.Layers.Background.HScrollTable.Values[0]} V={_renderService.Layers.Background.VScrollTable.Values[0]}";
    }
}