public record LayerGroup(Layer Background, Layer Foreground, Layer Window, Layer Sprites)
{
    public IEnumerable<Layer> Layers => [Background, Foreground, Window, Sprites];
}
