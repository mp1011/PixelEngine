public class LevelTileMap
{
    private Specs _specs;
    private byte[] _data;

    public ArrayDataGrid<Tile> Tiles { get; }

    public Size PixelSize => Tiles.Size * _specs.TileSize;

    public LevelTileMap(Size size, Specs specs) : this( new byte[size.Width * size.Height * 2], size, specs) 
    { }

    public LevelTileMap(byte[] data, Size size, Specs specs)
    {
        _specs = specs;
        _data = data;
        Tiles = new ArrayDataGrid<Tile>(size.Width, size.Height);

        int tileIndex = 0; 
        for (int i = 0; i < _data.Length; i += 2)
            Tiles[tileIndex++] = new Tile(_data,i);
    }

    public void SetFromOtherMap(int x, int y, Tile tile)
    {
        Tiles[x, y] = Tiles[x, y].CopyFrom(tile);
    }
}

