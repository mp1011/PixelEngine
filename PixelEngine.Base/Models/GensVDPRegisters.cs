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
    int SpriteTableLocation,
    int PlaneWidth,
    int PlaneHeight)
{
   
}