
internal class SFMLRenderService : RenderService<RGBAColor>
{
    private byte[] _pixelBuffer;
    private Texture _canvas;
    RectangleShape _shape;

    public SFMLRenderService(Specs specs, BitArrayDataGrid vram, LayerGroup layerGroup, Palette palette)
        : base(specs, vram, layerGroup, palette)
    {
        _pixelBuffer = new byte[specs.ScreenWidth * specs.ScreenHeight * 32];

        for(int i = 0; i < ColorData.Length; i++)
        {
            ColorData[i] = new RGBAColor(_pixelBuffer, i*4);
            ColorData[i].Init();
        }

        _canvas = new Texture((uint)specs.ScreenWidth, (uint)specs.ScreenHeight);
        _shape = new RectangleShape(new Vector2f(specs.ScreenWidth, specs.ScreenHeight));
        _shape.Position = new Vector2f(100, 100);
        _shape.Texture = _canvas;
    }

    protected override RGBAColor ToPlatformColor(int x, int y, Color c)
    {
        var color = new RGBAColor(_pixelBuffer, ((y * _specs.ScreenWidth) + x) * 4);
        color.R = c.R;
        color.G = c.G;
        color.B = c.B;
        return color;
    }

    public void Draw(RenderWindow renderWindow)
    {
        _canvas.Update(_pixelBuffer);
        renderWindow.Draw(_shape);
    }
}

