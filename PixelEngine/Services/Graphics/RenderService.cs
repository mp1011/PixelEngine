using PixelEngine.Models.Graphics.GensLike;

namespace PixelEngine.Services.Graphics;

public class RenderService
{
    private readonly Specs _specs;
    private readonly PatternTable _patternTable;
    public LayerGroup LayerGroup { get; }
    public ArrayDataGrid<VertexPositionColor> ColorData { get; }

    public Palette Palette { get; set; }

    public RenderService(Specs specs,
        PatternTable patternTable,
        LayerGroup layerGroup)
    {
        _patternTable = patternTable;
        _specs = specs;

        ColorData = new ArrayDataGrid<VertexPositionColor>(specs.ScreenWidth, specs.ScreenHeight);
        LayerGroup = layerGroup;
    }

    public void RefreshFrameColors()
    {
        for (int renderY = 0; renderY < _specs.ScreenHeight; renderY++)
        {
            DrawScanline(renderY);
        }
    }

    private void DrawScanline(int renderY)
    {
        DrawLayerScanline(renderY, LayerGroup.Background, true);
        DrawLayerScanline(renderY, LayerGroup.Foreground,false);
        DrawLayerScanline(renderY, LayerGroup.Window,false);
        DrawLayerScanline(renderY, LayerGroup.Sprites, false);

    }

    private void DrawLayerScanline(int renderY, Layer layer, bool isFirst)
    {
        WrappingPoint sourcePoint = new(layer.PixelSize);
       
        ColorData.ForEachInRow(renderY, (x, y) =>
        {
            sourcePoint.X = layer.Scroll.X + x;
            sourcePoint.Y = layer.Scroll.Y + y;

            Point tilePoint = sourcePoint.ToPoint().DivideBy(_specs.TileSize);
            var tile = layer.Tiles[tilePoint];

            var tileX = sourcePoint.X % _specs.TileSize;
            var tileY = sourcePoint.Y % _specs.TileSize;

            if (tile.Flags.HasFlag(TileFlags.FlipX))
                tileX = _specs.TileSize - tileX - 1;

            if (tile.Flags.HasFlag(TileFlags.FlipY))
                tileY = _specs.TileSize - tileY - 1;

            Color? pixelColor = _patternTable.GetTilePixel(tile, tileX, tileY, Palette);

            if(isFirst && pixelColor == null)
                pixelColor = Palette[0];

            if (pixelColor != null)
                ColorData[x, y] = new VertexPositionColor(new Vector3(x, y, 0), pixelColor.Value);

            //sourcePoint.X = layer.Scroll.X + x;
            //sourcePoint.Y = layer.Scroll.Y + y;

            //var sourceVal = layer.PixelData[sourcePoint.X, sourcePoint.Y];
            //if (sourceVal > 0 || isFirstLayer)
            //{
            //    ColorData[x, y] = new VertexPositionColor(new Vector3(x, y, 0), Palette[sourceVal]);
            //}
        });


        layer.RasterInterupts.ForEach(r => r.OnHBlank(layer, renderY));
    }
}
