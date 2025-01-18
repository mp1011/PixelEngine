public abstract class InputManager
{
    public InputManager(PlayerInput player1, PlayerInput player2)
    {
        Player1 = player1;
        Player2 = player2;
        MousePosition = new Point(0, 0);
    }

    public Point MousePosition { get; private set; }

    public PlayerInput Player1 { get; }
    public PlayerInput Player2 { get; }

    public void Update()
    {
        Player1.Update(GetKeys(0));
        Player2.Update(GetKeys(1));
        MousePosition = GetMousePosition();
    }

    protected abstract GamepadButtons GetKeys(int player);
    protected abstract Point GetMousePosition();
}

