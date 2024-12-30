public class MovingSprite
{
    private readonly Specs _specs;
    
    public double RealX { get; set; }
    public double RealY { get; set; }

    public int PixelWidth => (Sprite.HSize + 1) * _specs.TileSize;
    public int PixelHeight => (Sprite.VSize + 1) * _specs.TileSize;


    public MovingSprite(Sprite sprite, Specs specs)
    {
        _specs = specs;
        Sprite = sprite;
        HorizontalMotion = new AcceleratedMotion();
        VerticalMotion = new AcceleratedMotion();

        RealX = sprite.HorizontalPos;
        RealY = sprite.VerticalPos;
    }

    public Sprite Sprite { get; }

    public AcceleratedMotion HorizontalMotion { get; set; }
    public AcceleratedMotion VerticalMotion { get; set; }


    public void Update()
    {
        HorizontalMotion.Update();
        VerticalMotion.Update();

        RealX += HorizontalMotion.Speed;
        RealY += VerticalMotion.Speed;

        // may need something better
        Sprite.HorizontalPos = (int)RealX;
        Sprite.VerticalPos = (int)RealY;
    }
}

