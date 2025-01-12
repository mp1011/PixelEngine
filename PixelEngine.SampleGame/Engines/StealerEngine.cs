public record MemoryLocations(int? VRAM, int? CRAM, int? VSRAM, int? Registers)
{
    //29728192
    //14458776
    //29794016

    //  public static MemoryLocations Local => new MemoryLocations(23240128, 23305664, 23305952, 23307232);
    //  public static MemoryLocations Local => new MemoryLocations(33004992, 33070528, 33070816, Registers: 33072096);
    public static MemoryLocations Local => new MemoryLocations(29728192, 14458776, 29794016, Registers: 29795296);

   // public static MemoryLocations Local => new MemoryLocations(null, null, null, null);

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


