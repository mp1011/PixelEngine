class SfmlRenderService
{    
    private readonly RenderService _renderService;
    private readonly Texture _canvas;
    private RectangleShape _shape;
    private RenderWindow _window;
    private FrameRateDisplay _frameRateDisplay = new FrameRateDisplay();

    public Size WindowSize { get; set; } = new Size(640, 480);

    public Palette Palette(int index) => _renderService.Palette(index);

    public bool WindowIsOpen => _window.IsOpen;
    public void DispatchEvents() => _window.DispatchEvents();

    public SfmlRenderService(Specs specs, RenderService renderService)
    {
        _renderService = renderService;
        _canvas = new Texture((uint)specs.ScreenWidth, (uint)specs.ScreenHeight);

        _shape = new RectangleShape(new Vector2f(WindowSize.Width, WindowSize.Height));
        _shape.Position = new Vector2f(0, 0);
        _shape.Texture = _canvas;

        _window = new RenderWindow(
        new VideoMode((uint)WindowSize.Width, (uint)WindowSize.Height, 32),
        "TEST",
            Styles.Titlebar | Styles.Resize | Styles.Close);

        _window.SetFramerateLimit(60);
        _window.Closed += _window_Closed;
    }

    private void _window_Closed(object? sender, EventArgs e)
    {
        _window.Close();
    }

    public void DisplayFrame()
    {
        _canvas.Update(_renderService.CalculateFramePixels());
        _window.Clear();
        _window.Draw(_shape);
        _frameRateDisplay.Draw(_window);
        _window.Display();
    }
}

