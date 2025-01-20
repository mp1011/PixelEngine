class DiagnosticRenderService
{
    private Specs _specs;
    private readonly Texture _canvas;
    private RectangleShape _shape;
    private RenderService _renderService;
    private SfmlDiagnosticWindowManager _windowManager;
    private CoordinateTranslator _coordinateTranslator;
    private byte[] _pixelBuffer;

    private RectangleShape _screenBorder = new RectangleShape();
    private RectangleShape _screenBorderV = new RectangleShape();
    private RectangleShape _screenBorderH = new RectangleShape();
    private DebugString _debugString = new DebugString(0);


    public DiagnosticRenderService(Size renderSize, 
        RenderService renderService, 
        Specs specs, 
        SfmlDiagnosticWindowManager windowManager,
        CoordinateTranslator coordinateTranslator)
    {
        _specs = specs;
        _canvas = new Texture((uint)renderSize.Width, (uint)renderSize.Height);
        _windowManager = windowManager;
        _coordinateTranslator = coordinateTranslator;

        _shape = new RectangleShape(new Vector2f(renderSize.Width, renderSize.Height));
        _shape.Position = new Vector2f(0, 0);
        _shape.Texture = _canvas;
        _renderService = renderService;


        _screenBorder = CreateScreenBorderShape();
        _screenBorderV = CreateScreenBorderShape();
        _screenBorderH = CreateScreenBorderShape();

        _pixelBuffer = new byte[renderSize.Width * renderSize.Height * Color.Bytes];
    }

    private RectangleShape CreateScreenBorderShape()
    {
        var screenBorder = new RectangleShape(new Vector2f(_specs.ScreenWidth, _specs.ScreenHeight));
        screenBorder.OutlineColor = SFML.Graphics.Color.Blue;
        screenBorder.OutlineThickness = 2;
        screenBorder.FillColor = new SFML.Graphics.Color(0, 0, 0, 0);
        return screenBorder;
    }

    public void DrawPlane(ScrollingLayer layer, RenderWindow window)
    {
        var hScroll = layer.HScrollTable.Values[0];
        var vScroll = layer.VScrollTable.Values[0];

        var mouseScreenPos = _coordinateTranslator.PlaneToScreen(_windowManager.MousePos);
        var p = _coordinateTranslator.ScreenToPlane(mouseScreenPos);
        var w = _coordinateTranslator.ScreenToWorld(mouseScreenPos);
        _debugString.Text = $"Screen={mouseScreenPos} Plane={p} World={w}";

        _screenBorder.Position = new Vector2f((-hScroll).NMod(layer.PixelSize.Width), ((int)vScroll).NMod(layer.PixelSize.Height));
        _screenBorderV.Position = new Vector2f(_screenBorder.Position.X, _screenBorder.Position.Y - layer.PixelSize.Height);
        _screenBorderH.Position = new Vector2f(_screenBorder.Position.X - layer.PixelSize.Width, _screenBorder.Position.Y);

        _canvas.Update(_renderService.DiagnosticDrawPlane(layer, _pixelBuffer));

        window.Clear();
        window.Draw(_shape);
        window.Draw(_screenBorder);
        window.Draw(_screenBorderV);
        window.Draw(_screenBorderH);
        _debugString.Draw(window, 1,1);
        window.Display();
    }
}
