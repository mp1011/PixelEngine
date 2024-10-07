public abstract class RenderService
{
    protected readonly Specs _specs;
    private readonly BitArrayDataGrid _vram;
    public LayerGroup LayerGroup { get; }

    public Palette Palette { get; set; }


    public RenderService(Specs specs,
        BitArrayDataGrid vram,
        LayerGroup layerGroup,
        Palette palette)
    {
        _vram = vram;
        _specs = specs;

        LayerGroup = layerGroup;


        Palette = palette;
    }

    public abstract void RefreshFrameColors();

    protected Color? GetLayerPixel(Layer layer, bool isBase, int x, int y, WrappingPoint sourcePoint)
    {
        sourcePoint.X = layer.Scroll.X + x;
        sourcePoint.Y = layer.Scroll.Y + y;

        Point tilePoint = sourcePoint.ToPoint().DivideBy(_specs.TileSize);
        var tile = layer.Tiles[tilePoint];
        if (tile.X == 0 && tile.Y == 0)
            return null;

        var tileX = sourcePoint.X % _specs.TileSize;
        var tileY = sourcePoint.Y % _specs.TileSize;

        if (tile.Flags.HasFlag(TileFlags.FlipX))
            tileX = _specs.TileSize - tileX - 1;

        if (tile.Flags.HasFlag(TileFlags.FlipY))
            tileY = _specs.TileSize - tileY - 1;

        var xIndex = tile.X;
        var yIndex = tile.Y;

        var colorValue = _vram[(xIndex * _specs.TileSize) + tileX, (yIndex * _specs.TileSize) + tileY];
        if (colorValue == 0)
        {
            if (isBase)
                return Palette[0];
            else
                return null;
        }

        return Palette[colorValue];
    }
}

public abstract class RenderService<TColor> : RenderService
{
    private WrappingPoint _sourcePointBg;
    private WrappingPoint _sourcePointFg;
    private WrappingPoint _sourcePointWindow;
    private WrappingPoint _sourcePointSprites;

    protected RenderService(Specs specs, BitArrayDataGrid vram, LayerGroup layerGroup, Palette palette) :
        base(specs, vram, layerGroup, palette)
    {
        ColorData = new ArrayDataGrid<TColor>(specs.ScreenWidth, specs.ScreenHeight);

        _sourcePointBg = new(LayerGroup.Background.PixelSize);
        _sourcePointFg = new(LayerGroup.Foreground.PixelSize);
        _sourcePointWindow = new(LayerGroup.Window.PixelSize);
        _sourcePointSprites = new(LayerGroup.Sprites.PixelSize);
    }

    public ArrayDataGrid<TColor> ColorData { get; }

    protected abstract TColor ToPlatformColor(int x, int y, Color c);

    public override void RefreshFrameColors()
    {
        ColorData.ForEach((x, y) =>
        {
            ColorData[x, y] = ToPlatformColor(x, y,
                GetLayerPixel(LayerGroup.Sprites, false, x, y, _sourcePointSprites)
                ?? GetLayerPixel(LayerGroup.Window, false, x, y, _sourcePointWindow)
                ?? GetLayerPixel(LayerGroup.Foreground, false, x, y, _sourcePointFg)
                ?? GetLayerPixel(LayerGroup.Background, true, x, y, _sourcePointBg)
                ?? Palette[0]);
        });
    }
}
