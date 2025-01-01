/// <summary>
/// Sprite coordinates: 512x512, with visible area at 128,128
/// Screen coordinates: 0,0 = top left
/// World coordinates
/// </summary>
public class CoordinateTranslator
{
    private readonly ScrollingLayer _layer;
    private readonly Camera _camera;

    public Point ScrollSeamCorner { get; set; } = new Point(0,0);


    public CoordinateTranslator(ScrollingLayer layer, Camera camera)
    {
        _camera = camera;
        _layer = layer;
    }

    public Point SpriteToScreen(Sprite sprite) => SpriteToScreen(new Point(sprite.HorizontalPos, sprite.VerticalPos));
    public Point SpriteToScreen(Point spritePosition) => spritePosition - new Point(128, 128);

    public Point SpriteToWorld(Sprite sprite) => SpriteToWorld(new Point(sprite.HorizontalPos, sprite.VerticalPos));
    public Point SpriteToWorld(Point spritePosition)
    {
        var spriteScreenPosition = SpriteToScreen(spritePosition);
        return _camera.WorldLocation + spriteScreenPosition;
    }

    public Point WorldToSprite(Point worldPosition)
    {
        var screenPosition = worldPosition - _camera.WorldLocation;
        return screenPosition + new Point(128, 128);
    }
    
    public Point WorldToSprite(int worldX, int worldY) =>  WorldToSprite(new Point(worldX, worldY));
    public Point WorldToSprite(double worldX, double worldY) => WorldToSprite(new Point((int)worldX, (int)worldY));

}

