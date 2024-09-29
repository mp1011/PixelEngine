namespace PixelEngine.Services.Graphics;

public class RenderService
{

    public ArrayDataGrid<VertexPositionColor> ColorData { get; }

    public Palette Palette { get; set; }

    public RenderService(Specs specs)
    {
        ColorData = new ArrayDataGrid<VertexPositionColor>(specs.ScreenWidth, specs.ScreenHeight);
    }

    public void RefreshFrameColors(Layer[] layers)
    {
        bool isFirstLayer = true;
        foreach (Layer layer in layers)
        {
            WrappingPoint sourcePoint = new(layer.Size);
            int renderY = 0;

            layer.RasterInterupts.ForEach(r => r.OnHBlank(layer, 0));

            ColorData.ForEach((x, y) =>
            {
               if(y != renderY)
                {
                    renderY = y;
                    layer.RasterInterupts.ForEach(r => r.OnHBlank(layer, renderY));
                }

                sourcePoint.X = layer.Scroll.X + x;
                sourcePoint.Y = layer.Scroll.Y + y;

                var sourceVal = layer.PixelData[sourcePoint.X, sourcePoint.Y];
                if(sourceVal > 0 || isFirstLayer)
                {
                    ColorData[x, y] = new VertexPositionColor(new Vector3(x, y, 0), Palette[sourceVal]);                   
                }
            });

            isFirstLayer = false;
        }
    }
}
