public class Layer
{
    private readonly Specs _specs;

    public WrappingPoint Scroll { get; }

    public DataGrid<Tile> Tiles { get; }

    public Size TileSize => Tiles.Size;
    public Size PixelSize => Tiles.Size * _specs.TileSize;
    public List<IRasterInterupt> RasterInterupts { get; } = new List<IRasterInterupt>();

    public Layer(Specs specs, int tilesX, int tilesY)
    {
        _specs = specs;
        Tiles = new ArrayDataGrid<Tile>(tilesX, tilesY);
        Scroll = new WrappingPoint(PixelSize.Width, PixelSize.Height);
    }
}
