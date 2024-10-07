class XnaRenderService : RenderService<VertexPositionColor>
{
    public XnaRenderService(Specs specs, BitArrayDataGrid vram, LayerGroup layerGroup, Palette palette) 
        : base(specs, vram, layerGroup, palette)
    {
    }

    protected override VertexPositionColor ToPlatformColor(int x, int y, Color c) => new VertexPositionColor(
        new Vector3(x, y, 0), c.ToXnaColor());
}
