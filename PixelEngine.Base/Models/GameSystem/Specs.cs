public record struct Specs(
    int ScreenWidth,
    int ScreenHeight,
    int BitsPerPixel,
    int TileSize,
    int ColorsPerPalette,
    int NumPalettes,
    int NumSprites)
{
    public static Specs GensLike => new Specs(
        ScreenWidth: 320, 
        ScreenHeight: 224,
        BitsPerPixel: 4, 
        TileSize: 8, 
        ColorsPerPalette: 16, 
        NumPalettes: 4, 
        NumSprites: 80);
}