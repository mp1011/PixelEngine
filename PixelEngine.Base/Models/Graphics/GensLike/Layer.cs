public class Layer
{
    private readonly Specs _specs;

    public WrappingPoint Scroll { get; }

    public DataGrid<Tile> Tiles { get; }

    public Size TileSize => Tiles.Size;
    public Size PixelSize => Tiles.Size * _specs.TileSize;
    public List<IRasterInterupt> RasterInterupts { get; } = new List<IRasterInterupt>();

    public Layer(Specs specs)
    {
        _specs = specs;
        //PixelData = new BitArrayDataGrid(specs.ScreenWidth, specs.ScreenHeight,
        //    new NBitArray(specs.BitsPerPixel, specs.ScreenWidth * specs.ScreenHeight));

        Tiles = new ArrayDataGrid<Tile>(64, 64);

        Scroll = new WrappingPoint(PixelSize.Width, PixelSize.Height);
    }
}
