public record LayerGroup(ScrollingLayer Background, ScrollingLayer Foreground, Layer Window, Layer Sprites)
{
    public IEnumerable<Layer> Layers => [Background, Foreground, Window, Sprites];
}
