public enum HScrollMode : byte
{
    FullScreen = 0,
    Invalid = 1,
    EightPixelStrips = 2,
    Line = 3
}

public enum VScrollMode : byte
{
    FullScreen = 0,
    EightPixelStrips = 1,
}

public record GensVDPRegisters(
    HScrollMode HScrollMode,
    VScrollMode VScrollMode,
    int HScrollLocation,
    int PlaneALocation,
    int PlaneBLocation,
    int SpriteTableLocation) 
{
    public static GensVDPRegisters KC => new GensVDPRegisters(
        HScrollMode.Line,
        VScrollMode.FullScreen,
        0x1400,
        0,
        0xE000,
        0x1000);

    public static GensVDPRegisters Ecco => new GensVDPRegisters(
       HScrollMode.FullScreen,
       VScrollMode.FullScreen,
       0xFC00,
       0xC000,
       0xE000,
       0xF800);

    public static GensVDPRegisters Sonic => new GensVDPRegisters(
      HScrollMode.Line,
      VScrollMode.FullScreen,
      0xFC00,
      0xC000,
      0xE000,
      0xF800);
}