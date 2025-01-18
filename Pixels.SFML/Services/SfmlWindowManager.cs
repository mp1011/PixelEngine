class SfmlWindowManager
{
    private readonly Specs _specs;

    private readonly SfmlInputManager _inputManager;
    public RenderWindow Window { get; }
    public Size WindowSize { get; set; } = new Size(640, 480);

    public SfmlWindowManager(SfmlInputManager inputManager, Specs specs)
    {
        _specs = specs;
        _inputManager = inputManager;
        Window = new RenderWindow(
        new VideoMode((uint)WindowSize.Width, (uint)WindowSize.Height, 32),
        "TEST",
            Styles.Titlebar | Styles.Resize | Styles.Close);

        Window.SetFramerateLimit(60);
        Window.Closed += Window_Closed;
        Window.KeyPressed += Window_KeyPressed;
        Window.KeyReleased += Window_KeyReleased;
        Window.MouseMoved += Window_MouseMoved;
        Window.MouseButtonPressed += Window_MouseButtonPressed;
        Window.MouseButtonReleased += Window_MouseButtonReleased;
    }

    private void Window_MouseButtonReleased(object? sender, MouseButtonEventArgs e)
    {

        var keys = _inputManager.PlayerKeys[0];
        if (e.Button == Mouse.Button.Left)
            keys = keys & ~GamepadButtons.A;
        if (e.Button == Mouse.Button.Right)
            keys = keys & ~GamepadButtons.B;

        _inputManager.PlayerKeys[0] = keys;
    }

    private void Window_MouseButtonPressed(object? sender, MouseButtonEventArgs e)
    {
        var keys = _inputManager.PlayerKeys[0];
        if (e.Button == Mouse.Button.Left)
            keys = keys | GamepadButtons.A;
        if (e.Button == Mouse.Button.Right)
            keys = keys | GamepadButtons.B;

        _inputManager.PlayerKeys[0] = keys;
    }

    private void Window_MouseMoved(object? sender, MouseMoveEventArgs e)
    {
        var windowCoords = Window.MapPixelToCoords(new Vector2i(e.X, e.Y));
        var pctX = windowCoords.X / WindowSize.Width;
        var pctY = windowCoords.Y / WindowSize.Height;

        _inputManager.CurrentMousePosition = new Point(
            (int)(pctX * _specs.ScreenWidth),
            (int)(pctY * _specs.ScreenHeight));
    }

    private void Window_KeyReleased(object? sender, KeyEventArgs e)
    {
        var keys = _inputManager.PlayerKeys[0];
        if (e.Code == Keyboard.Key.Left)
            keys = keys & ~GamepadButtons.Left;
        if (e.Code == Keyboard.Key.Right)
            keys = keys & ~GamepadButtons.Right;
        if (e.Code == Keyboard.Key.Up)
            keys = keys & ~GamepadButtons.Up;
        if (e.Code == Keyboard.Key.Down)
            keys = keys & ~GamepadButtons.Down;
        if (e.Code == Keyboard.Key.A)
            keys = keys & ~GamepadButtons.A;
        if (e.Code == Keyboard.Key.S)
            keys = keys & ~GamepadButtons.B;
        _inputManager.PlayerKeys[0] = keys;
    }

    private void Window_KeyPressed(object? sender, KeyEventArgs e)
    {
        var keys = _inputManager.PlayerKeys[0];
        if (e.Code == Keyboard.Key.Left)
            keys = keys | GamepadButtons.Left;
        if (e.Code == Keyboard.Key.Right)
            keys = keys | GamepadButtons.Right;
        if (e.Code == Keyboard.Key.Up)
            keys = keys | GamepadButtons.Up;
        if (e.Code == Keyboard.Key.Down)
            keys = keys | GamepadButtons.Down;
        if (e.Code == Keyboard.Key.A)
            keys = keys | GamepadButtons.A;
        if (e.Code == Keyboard.Key.S)
            keys = keys | GamepadButtons.B;

        _inputManager.PlayerKeys[0] = keys;
    }

    public void DispatchEvents() => Window.DispatchEvents();

    private void Window_Closed(object? sender, EventArgs e)
    {
        Window.Close();
    }
}

