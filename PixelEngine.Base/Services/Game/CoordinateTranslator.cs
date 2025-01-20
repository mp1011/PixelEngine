/// <summary>
/// Sprite coordinates: 512x512, with visible area at 128,128
/// Screen coordinates: 0,0 = top left
/// World coordinates
/// </summary>
public class CoordinateTranslator
{
    private readonly Specs _specs;
    private readonly ScrollingLayer _layer;
    private readonly LevelTileMap _levelMap;
    private readonly Camera _camera;

    public Point ScrollSeamCorner { get; set; } = new Point(0,0);


    public CoordinateTranslator(ScrollingLayer layer, LevelTileMap levelTileMap, Camera camera, Specs specs)
    {
        _specs = specs;
        _camera = camera;
        _layer = layer;
        _levelMap = levelTileMap;
    }

    public Point ScreenToPlane(int x, int y) => ScreenToPlane(new Point(x,y));

    public Point ScreenToPlane(Point screenPoint) => screenPoint
        .Add(-_layer.HScrollTable.Values[0], _layer.VScrollTable.Values[0])
        .NMod(_layer.PixelSize);

    public Point PlaneToScreen(Point planePoint)
    {
        var screenPlanePos = ScreenToPlane(0, 0);
        var screenPoint = planePoint.Add(-screenPlanePos.X, -screenPlanePos.Y);
        return screenPoint;
    }

    public Point PlaneToWorld(Point planePoint)
    {
        var screenPoint = PlaneToScreen(planePoint);
        return ScreenToWorld(screenPoint);
    }

    public Point WorldToPlane(Point worldPoint)
    {
        var screenPos = worldPoint - _camera.WorldLocation;
        return ScreenToPlane(screenPos);
    }

    public Point WorldToScreen(Point worldPoint)
    {
        var plane = WorldToPlane(worldPoint);
        return PlaneToScreen(plane);
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
    public Point ScreenToWorld(Point screenPosition) => (_camera.WorldLocation + screenPosition).NMod(_levelMap.PixelSize);
}

