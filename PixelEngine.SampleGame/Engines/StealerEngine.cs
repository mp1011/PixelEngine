public record MemoryLocations(int? VRAM, int? CRAM, int? VSRAM, int? Registers)
{
    // 25596298;
    public static MemoryLocations Local => new MemoryLocations(23240128, 23305664, 23305952, 23307232);
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



        if (_mode == StealerMode.PlaneExtractor)
            _planeStealer = new PlaneStealer(_renderService.Layers.Foreground, _inputManager, _specs);
    }

    public override void Update(ulong frameNumber)
    {
        var vram = _stealer.CurrentVram();
        if (vram.Length == 0)
            return;

        var registers = GensSaveStateImporter.ReadRegisters(_stealer.CurrentRegisters(), 0);
        GensSaveStateImporter.ReadVram(
            _renderService,
            vram,
            _stealer.CurrentVsram(),
            _stealer.CurrentCram(),
            registers,
            _specs);

        switch(_mode)
        {
            case StealerMode.Mirror:
                var fh = _renderService.Layers.Foreground.HScrollTable.Values[0];
                var fv = _renderService.Layers.Foreground.VScrollTable.Values[0];
                var bh = _renderService.Layers.Background.HScrollTable.Values[0];
                var bv = _renderService.Layers.Background.VScrollTable.Values[0];

                var b0 = vram[0x1400];
                var b1 = (vram[0x1400 + 1] & 3);
                var b2 = vram[0x1400 + 2];
                var b3 = (vram[0x1400 + 3] & 3);

                Debug.Text1 = $"FG={fh},{fv}  BG={bh},{bv}";
                Debug.Text2 = $"{b0} | {b1} | {b2} | {b3}";
                break;
            case StealerMode.PlaneExtractor:
                _planeStealer.Update();
                break;
        }

    }
}


