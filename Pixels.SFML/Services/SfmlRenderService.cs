class SfmlRenderService
{    
    private readonly RenderService _renderService;
    private readonly Texture _canvas;
    private RectangleShape _shape;

    private readonly SfmlWindowManager _windowManager;    
    private List<IDisplayOverlay> _overlays = new List<IDisplayOverlay>();
   
    public SfmlRenderService(Specs specs, SfmlWindowManager windowManager, RenderService renderService)
    {
        _renderService = renderService;
        _windowManager = windowManager;
        _canvas = new Texture((uint)specs.ScreenWidth, (uint)specs.ScreenHeight);

        _shape = new RectangleShape(new Vector2f(windowManager.WindowSize.Width, windowManager.WindowSize.Height));
        _shape.Position = new Vector2f(0, 0);
        _shape.Texture = _canvas;
    }

    public void AddOverlay(IDisplayOverlay overlay)
    {
        _overlays.Add(overlay);
    }

    public void DisplayFrame()
    {
        var window = _windowManager.Window;
        _canvas.Update(_renderService.CalculateFramePixels());
        window.Clear();
        window.Draw(_shape);

        foreach (var overlay in _overlays)
            overlay.Draw(window);

        window.Display();
    }
}

