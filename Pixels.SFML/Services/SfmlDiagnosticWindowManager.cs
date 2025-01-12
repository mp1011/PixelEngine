class SfmlDiagnosticWindowManager
{
    public SfmlDiagnosticWindowManager()
    {
        Window = new RenderWindow(
        new VideoMode((uint)WindowSize.Width, (uint)WindowSize.Height, 32),
        "TEST",
            Styles.Titlebar | Styles.Resize | Styles.Close);

        Window.SetFramerateLimit(60);
        Window.Closed += Window_Closed;
        Window.MouseButtonPressed += Window_MouseButtonPressed;
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

