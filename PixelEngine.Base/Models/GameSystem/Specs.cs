public record struct Specs(
    int ScreenWidth,
    int ScreenHeight,
    int BitsPerPixel,
    int TileSize,
    int PatternTableTilesAcross,
    int ColorsPerPalette,
    int NumPalettes,
    int NumSprites)
{
    public static Specs GensLike => new Specs(320, 224, 4, 8, 16, 16, 4, 80);
}