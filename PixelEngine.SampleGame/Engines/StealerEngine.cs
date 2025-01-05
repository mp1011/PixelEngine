public record MemoryLocations(int? VRAM, int? CRAM, int? VSRAM)
{
    public static MemoryLocations Local => new MemoryLocations(23240128, 23305664, 23305952);
    public static MemoryLocations Unknown => new MemoryLocations(null, null, null);
}

public class StealerEngine : Engine
{
    private VramStealer _stealer;
    private GensVDPRegisters _gensVDPRegisters;
    private MemoryLocations _memoryLocations;

    public StealerEngine(GensVDPRegisters vdpRegisters, MemoryLocations memoryLocations, RenderService renderService, InputManager inputManager, Specs specs)
        : base(renderService, inputManager, specs)
    {
        _stealer = new VramStealer();
        _gensVDPRegisters = vdpRegisters;
        _memoryLocations = memoryLocations;
    }

    public override void Load()
    {               
        _stealer.Setup(_memoryLocations.VRAM.GetValueOrDefault(), _memoryLocations.CRAM.GetValueOrDefault(), _memoryLocations.VSRAM.GetValueOrDefault());

        if (_memoryLocations.VRAM == null)
            _stealer.ScanForVram();

        if (_memoryLocations.CRAM == null)
            _stealer.ScanForCram();

        if (_memoryLocations.VSRAM == null)
            _stealer.ScanForVsRam();
        //_stealer.ScanForCram();
       // _stealer.FindVsRam();
    }

    public override void Update(ulong frameNumber)
    {
        var vram = _stealer.CurrentVram();
        if (vram.Length == 0)
            return;
           
        GensSaveStateImporter.ReadVram(
            _renderService,
            vram,
            _stealer.CurrentVsram(),
            _stealer.CurrentCram(),
            (byte)_gensVDPRegisters.HScrollMode,
            (byte)_gensVDPRegisters.VScrollMode,
            _gensVDPRegisters.HScrollLocation,
            _gensVDPRegisters.PlaneALocation,
            _gensVDPRegisters.PlaneBLocation,
            _gensVDPRegisters.SpriteTableLocation,
            _specs);

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
    }
}


