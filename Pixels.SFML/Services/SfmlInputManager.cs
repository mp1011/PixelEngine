class SfmlInputManager : InputManager
{
    public GamepadButtons[] PlayerKeys { get;  }

    public SfmlInputManager(PlayerInput player1, PlayerInput player2) : base(player1, player2)
    {
        PlayerKeys = new GamepadButtons[2];
    }

    protected override GamepadButtons GetKeys(int player)
    {
        return PlayerKeys[player];
    }
}

