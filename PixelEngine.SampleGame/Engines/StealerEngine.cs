public record MemoryLocations(int? VRAM, int? CRAM, int? VSRAM, int? Registers)
{
    private const string _filename = "gens_mem.bin";

    public static MemoryLocations FromDisk()
    {
        if (File.Exists(_filename))
        {
            var data = File.ReadAllBytes("gens_mem.bin");
            return new MemoryLocations(
                BitConverter.ToInt32(data, 0),
                BitConverter.ToInt32(data, 4),
                BitConverter.ToInt32(data, 8),
                BitConverter.ToInt32(data, 12));
        }

        return new MemoryLocations(null, null, null, null);
    }

    public void WriteToDisk()
    {
        List<byte> data = new List<byte>();
        data.AddRange(BitConverter.GetBytes(VRAM!.Value));
        data.AddRange(BitConverter.GetBytes(CRAM!.Value));
        data.AddRange(BitConverter.GetBytes(VSRAM!.Value));
        data.AddRange(BitConverter.GetBytes(Registers!.Value));

        File.WriteAllBytes(_filename, data.ToArray());
    }
}

public enum StealerMode
{
    Mirror,
    PlaneExtractor,
}

public class StealerEngine : Engine
{
    private VramStealer _stealer;
    private MemoryLocations _memoryLocations;
    private StealerMode _mode;

    private PlaneStealer _planeStealer;


    public StealerEngine(StealerMode mode, MemoryLocations memoryLocations, RenderService renderService, InputManager inputManager, Specs specs)
        : base(renderService, inputManager, specs)
    {
        _stealer = new VramStealer();
        _memoryLocations = memoryLocations;
        _mode = mode;
    }

    public override void Load()
    {
        _stealer.Setup(
            _memoryLocations.VRAM.GetValueOrDefault(),
            _memoryLocations.CRAM.GetValueOrDefault(),
            _memoryLocations.VSRAM.GetValueOrDefault(),
            _memoryLocations.Registers.GetValueOrDefault());

        if (_memoryLocations.VRAM == null)
            _stealer.ScanForVram();

        if (_memoryLocations.CRAM == null)
            _stealer.ScanForCram();

        if (_memoryLocations.VSRAM == null)
            _stealer.ScanForVsRam();

        if (_memoryLocations.Registers == null)
            _stealer.ScanForRegisters();

        _memoryLocations = new MemoryLocations(_stealer.VramAddress, _stealer.CramAddress, _stealer.VsramAddress, _stealer.RegistersAddress);
        _memoryLocations.WriteToDisk();

        if (_mode == StealerMode.PlaneExtractor)
            _planeStealer = new PlaneStealer(_renderService.Layers.Foreground, _inputManager, _specs);
    }

    public override void Update(ulong frameNumber)
    {
        var vram = _stealer.CurrentVram();
        if (vram.Length == 0)
            return;

        var registers = GensSaveStateImporter.ReadRegisters(_stealer.CurrentRegisters(), 0);
        _renderService.SetAllData(registers, vram, _stealer.CurrentCram(), _stealer.CurrentVsram());

     
        switch(_mode)
        {
            case StealerMode.Mirror:                
                Debug.Text1 = $"A=Save State";
                if(_inputManager.Player1.KeyPressed(GamepadButtons.A))
                {
                    SaveRenderState();
                }

                break;
            case StealerMode.PlaneExtractor:
                _planeStealer.Update();
                break;
        }
    }

    private void SaveRenderState()
    {
        List<byte> all = new List<byte>();
        all.AddRange(_stealer.CurrentRegisters());
        all.AddRange(_stealer.CurrentVram());
        all.AddRange(_stealer.CurrentCram());
        all.AddRange(_stealer.CurrentVsram());

        File.WriteAllBytes("renderstate.ram", all.ToArray());
    }
}


