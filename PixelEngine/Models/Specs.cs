namespace PixelEngine.Models;

public class Specs
{
    public int ScreenWidth { get; } = 320;
    public int ScreenHeight { get; } = 224;

    public int BitsPerPixel { get; } = 4;
    public int TileSize { get; } = 8;

    public int PatternTableTilesAcross { get; } = 16;

}
