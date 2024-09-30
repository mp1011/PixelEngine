namespace PixelEngine.Models.Graphics.GensLike;

public record LayerGroup(Layer Background, Layer Foreground, Layer Window, Layer Sprites)
{
    public LayerGroup(Specs specs) : this(
        new Layer(specs),
        new Layer(specs),
        new Layer(specs),
        new Layer(specs))
    { }
}
