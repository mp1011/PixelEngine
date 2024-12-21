public record struct Specs(int ScreenWidth, int ScreenHeight, int BitsPerPixel, int TileSize, int PatternTableTilesAcross)
{
    public static Specs GensLike => new Specs(320, 224, 4, 8, 16);
}