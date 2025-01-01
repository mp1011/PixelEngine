public class StreamScroller
{
    private Specs _specs;
    private ScrollingLayer _background;
    private ScrollingLayer _foreground;
    
    private BackgroundScrollLayer[] _backgroundHScrollLayers;
    private BackgroundScrollLayer[] _backgroundVScrollLayers;

    private Camera _camera;
    private MovingSprite _focus;
    private CoordinateTranslator _coordinateTranslator;
    private Point _centerLocation;

    public StreamScroller(ScrollingLayer background, 
        ScrollingLayer foreground, 
        MovingSprite focus, 
        CoordinateTranslator coordinateTranslator, 
        Camera camera,
        BackgroundScrollLayer[] backgroundHScrollLayers,
        BackgroundScrollLayer[] backgroundVScrollLayers,
        Specs specs)
    {
        _specs = specs;
        _background = background;
        _foreground = foreground;
        _backgroundHScrollLayers = backgroundHScrollLayers;
        _backgroundVScrollLayers = backgroundVScrollLayers;
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
        
        _foreground.HScrollTable.SetAll((ushort)_camera.WorldLocation.X);
        _foreground.VScrollTable.SetAll((ushort)_camera.WorldLocation.Y);

        int line = 0;
        ushort vScroll = 0;
        foreach (var bgScrollLayer in _backgroundVScrollLayers)
        {
            vScroll = (ushort)(_camera.WorldLocation.Y * bgScrollLayer.ScrollFactor);
            _background.VScrollTable.SetRange(line, bgScrollLayer.Lines, vScroll);
            line += bgScrollLayer.Lines;
        }

        line = 0;
        foreach (var bgScrollLayer in _backgroundHScrollLayers)
        {
            int adjustedLine = line - vScroll;
            _background.HScrollTable.SetRange(adjustedLine, bgScrollLayer.Lines, (ushort)(_camera.WorldLocation.X * bgScrollLayer.ScrollFactor));
            line += bgScrollLayer.Lines;
        }



    }
}

