class SfmlWindowManager
{
    public SfmlWindowManager(SfmlInputManager inputManager)
    {
        _inputManager = inputManager;
        Window = new RenderWindow(
        new VideoMode((uint)WindowSize.Width, (uint)WindowSize.Height, 32),
        "TEST",
            Styles.Titlebar | Styles.Resize | Styles.Close);

        Window.SetFramerateLimit(60);
        Window.Closed += Window_Closed;
        Window.KeyPressed += Window_KeyPressed;
        Window.KeyReleased += Window_KeyReleased;
    }

    private void Window_KeyReleased(object? sender, KeyEventArgs e)
    {
        var keys = _inputManager.PlayerKeys[0];
        if (e.Code == Keyboard.Key.Left)
            keys = keys & ~GamepadButtons.Left;
        if (e.Code == Keyboard.Key.Right)
            keys = keys & ~GamepadButtons.Right;
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
        if (e.Code == Keyboard.Key.A)
            keys = keys | GamepadButtons.A;
        if (e.Code == Keyboard.Key.S)
            keys = keys | GamepadButtons.B;

        _inputManager.PlayerKeys[0] = keys;
    }

    private SfmlInputManager _inputManager;
    public RenderWindow Window { get; }   
    public Size WindowSize { get; set; } = new Size(640, 480);

    public void DispatchEvents() => Window.DispatchEvents();

    private void Window_Closed(object? sender, EventArgs e)
    {
        Window.Close();
    }
}

