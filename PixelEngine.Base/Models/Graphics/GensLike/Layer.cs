using System.Reflection.Emit;

public class Layer
{
    protected readonly Specs _specs;

    public DataGrid<Tile> Tiles { get; private set; }

    public Size TileSize => Tiles.Size;
    public Size PixelSize => Tiles.Size * _specs.TileSize;
    public List<IRasterInterupt> RasterInterupts { get; } = new List<IRasterInterupt>();

    public Layer(Specs specs, int tilesX, int tilesY)
    {
        _specs = specs;
        Tiles = new ArrayDataGrid<Tile>(tilesX, tilesY);
    }

    public void Resize(Size tiles)
    {
        Tiles = new ArrayDataGrid<Tile>(tiles.Width, tiles.Height);
    }

    public void SetData(byte[] vram, int address)
    {
        for (int i = 0; i < Tiles.Length; i++)
        {
            Tiles[i] = new Tile(vram, address);
            address += 2;
        }
    }
}

public class ScrollingLayer : Layer
{
    public ScrollingLayer(Specs specs, int tilesX, int tilesY) : base(specs, tilesX, tilesY)
    {
        HScrollTable = new ScrollTable(ScrollTableType.FullScreen, true, PixelSize.Width, specs);
        VScrollTable = new ScrollTable(ScrollTableType.FullScreen, false, PixelSize.Height, specs);
    }

    public ScrollTable HScrollTable { get; set; }
    public ScrollTable VScrollTable { get; set; }
}
