public class CoordinateTranslator
{
    private readonly ScrollingLayer _layer;

    public Point ScrollSeamCorner { get; set; } = new Point(0,0);


    public CoordinateTranslator(ScrollingLayer layer)
    {
        _layer = layer;
    }

    public Point SpriteToWorld(Sprite sprite) => SpriteToWorld(new Point(sprite.HorizontalPos, sprite.VerticalPos));

    public Point SpriteToWorld(Point spritePosition)
    {
        var spriteScreenPosition = spritePosition - new Point(128, 128);

        //var layerScreenPosition = new Point(
        //    _layer.HScrollTable.Values[0],
        //    _layer.VScrollTable.Values[0]);
       
        // todo, implement given position of layer scroll seam
        return spriteScreenPosition;
    }

    public Point WorldToSprite(Point worldPosition)
    {
        return worldPosition + new Point(128, 128);
        // todo, fully implement
    }
    
}

