class SfmlDiagnosticWindowManager
{
    public Point MousePos { get; private set; } = new Point(0,0);

    public SfmlDiagnosticWindowManager()
    {
        Window = new RenderWindow(
        new VideoMode((uint)WindowSize.Width, (uint)WindowSize.Height, 32),
        "TEST",
            Styles.Titlebar | Styles.Resize | Styles.Close);

        Window.SetFramerateLimit(60);
        Window.Closed += Window_Closed;
        Window.MouseMoved += Window_MouseMoved;
    }

    private void Window_MouseMoved(object? sender, MouseMoveEventArgs e)
    {
        var m = Window.MapPixelToCoords(new Vector2i(e.X, e.Y));
        MousePos = new Point((int)m.X, (int)m.Y);
    }

    private void Window_MouseButtonPressed(object? sender, MouseButtonEventArgs e)
    {

    }

    public RenderWindow Window { get; }
    public Size WindowSize { get; set; } = new Size(640, 480);

    public void DispatchEvents() => Window.DispatchEvents();

    private void Window_Closed(object? sender, EventArgs e)
    {
        Window.Close();
    }
}

