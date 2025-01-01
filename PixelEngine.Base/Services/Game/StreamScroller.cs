public class StreamScroller
{
    private Specs _specs;
    private ScrollingLayer _background;
    private ScrollingLayer _foreground;
    private Camera _camera;
    private MovingSprite _focus;
    private CoordinateTranslator _coordinateTranslator;
    private Point _centerLocation;

    public StreamScroller(ScrollingLayer background, ScrollingLayer foreground, MovingSprite focus, CoordinateTranslator coordinateTranslator, Camera camera, Specs specs)
    {
        _specs = specs;
        _background = background;
        _foreground = foreground;
        _focus = focus;
        _camera = camera;
        _coordinateTranslator = coordinateTranslator;    
        _centerLocation = new Point(specs.ScreenWidth / 2, specs.ScreenHeight / 2);
        _centerLocation -= new Point(focus.PixelWidth / 2, focus.PixelHeight / 2);
    }

    public void Update()
    {
        // todo, calc this offset
        _camera.WorldLocation = new Point((int)_focus.WorldX - 160, (int)_focus.WorldY - 128);   
        
        _foreground.HScrollTable.SetAll((short)_camera.WorldLocation.X);
        _foreground.VScrollTable.SetAll((short)_camera.WorldLocation.Y);

        _background.HScrollTable.SetAll((short)(_camera.WorldLocation.X * 0.5));
        _background.VScrollTable.SetAll((short)(_camera.WorldLocation.Y * 0.5));
    }
}

