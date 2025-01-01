public class MovingSprite
{
    private readonly Specs _specs;
    private readonly CoordinateTranslator _coordinateTranslator;

    public double WorldX { get; set; }
    public double WorldY { get; set; }
    public Point WorldLocation => new Point((int)WorldX, (int)WorldY);

    public int PixelWidth => (Sprite.HSize + 1) * _specs.TileSize;
    public int PixelHeight => (Sprite.VSize + 1) * _specs.TileSize;


    public MovingSprite(Sprite sprite, CoordinateTranslator coordinateTranslator, Specs specs)
    {
        _specs = specs;
        _coordinateTranslator = coordinateTranslator;
        Sprite = sprite;
        HorizontalMotion = new AcceleratedMotion();
        VerticalMotion = new AcceleratedMotion();

        var worldPos = _coordinateTranslator.SpriteToWorld(new Point(sprite.HorizontalPos, sprite.VerticalPos));
        WorldX = worldPos.X;
        WorldY = worldPos.Y;
    }

    public Sprite Sprite { get; }

    public AcceleratedMotion HorizontalMotion { get; set; }
    public AcceleratedMotion VerticalMotion { get; set; }


    public void Update()
    {
        HorizontalMotion.Update();
        VerticalMotion.Update();

        WorldX += HorizontalMotion.Speed;
        WorldY += VerticalMotion.Speed;

        var spritePosition = _coordinateTranslator.WorldToSprite(WorldX, WorldY);
        Sprite.HorizontalPos = spritePosition.X;
        Sprite.VerticalPos = spritePosition.Y;
    }
}

