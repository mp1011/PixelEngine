using PixelEngine.Base.Vram;

class XnaRenderService : RenderService
{
    public XnaRenderService(Specs specs, LayerGroup layerGroup, Palette palette) 
        : base(specs, layerGroup, palette)
    {
    }

    protected override Vram InitVram()
    {
        throw new System.NotImplementedException();
    }
}
