
using PixelEngine.Base.Vram;
using SFML.Window;

internal class SFMLRenderService : RenderService
{  
    private RenderWindow _renderWindow;
    public SFMLRenderService(RenderWindow window, Specs specs, LayerGroup layerGroup, Palette palette)
        : base(specs, layerGroup, palette)
    {
        _renderWindow = window;
    }

    protected override Vram InitVram() =>
        new Vram(LayerGroup.Layers.Select(p => new SfmlVramLayer(_renderWindow, p.PixelSize.Width, p.PixelSize.Height)));


    public void Draw(RenderWindow renderWindow)
    {
        foreach(var layer in Vram.Layers)
        {
            layer.ApplyBuffer();
            layer.Draw();
        }
    }
}

