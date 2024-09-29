using PixelEngine.Services.Graphics;

namespace PixelEngine.Models.Graphics;

public class Layer
{
    public WrappingPoint Scroll { get; }
    public DataGrid<byte> PixelData { get; }
    public Point Size => PixelData.Size;
    public List<IRasterInterupt> RasterInterupts { get; } = new List<IRasterInterupt>();

    public Layer(Specs specs)
    {
        PixelData = new BitArrayDataGrid(specs.ScreenWidth, specs.ScreenHeight,
            new NBitArray(specs.BitsPerPixel, specs.ScreenWidth * specs.ScreenHeight));

        Scroll = new WrappingPoint(PixelData.Width, PixelData.Height);
    }
}
