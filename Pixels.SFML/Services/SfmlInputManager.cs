class SfmlInputManager : InputManager
{
    public GamepadButtons[] PlayerKeys { get; }

    public Point CurrentMousePosition { get; set; }

    public SfmlInputManager(PlayerInput player1, PlayerInput player2) : base(player1, player2)
    {
        PlayerKeys = new GamepadButtons[2];
        CurrentMousePosition = new Point(0, 0);
    }

    protected override GamepadButtons GetKeys(int player)
    {
        return PlayerKeys[player];
    }

    protected override Point GetMousePosition() => CurrentMousePosition;
}

