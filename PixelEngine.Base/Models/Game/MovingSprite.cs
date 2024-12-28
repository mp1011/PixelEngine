public class MovingSprite
{    
    public double RealX { get; set; }
    public double RealY { get; set; }

    public MovingSprite(Sprite sprite)
    {
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

