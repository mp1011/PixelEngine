namespace PixelEngine.Models.Graphics.GensLike;

public class PatternTable
{
    private readonly BitArrayDataGrid _vram;
    private readonly Specs _specs;

    public PatternTable(byte[] vramData, Specs specs)
    {
        _specs = specs;
        _vram = GensVramImporter.Import(vramData);
    }

    public Color? GetTilePixel(Tile tile, int tileX, int tileY, Palette palette)
    {
        if (tile.Index == 0)
            return null;

        var xIndex = tile.Index % _specs.PatternTableTilesAcross;
        var yIndex = tile.Index / _specs.PatternTableTilesAcross;

        var colorValue = _vram[(xIndex * _specs.TileSize) + tileX, (yIndex * _specs.TileSize) + tileY];
        if (colorValue == 0)
            return null;
        return palette[colorValue];
    }

}
