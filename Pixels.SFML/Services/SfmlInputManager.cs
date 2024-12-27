class SfmlInputManager<TButtons> : InputManager<TButtons> where TButtons : struct
{
    public TButtons[] PlayerKeys { get;  }

    public SfmlInputManager(PlayerInput<TButtons> player1, PlayerInput<TButtons> player2) : base(player1, player2)
    {
        PlayerKeys = new TButtons[2];
    }

    protected override TButtons GetKeys(int player)
    {
        return PlayerKeys[player];
    }
}

