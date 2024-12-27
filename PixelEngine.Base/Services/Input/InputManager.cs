public abstract class InputManager<TKeys> where TKeys : struct
{
    public InputManager(PlayerInput<TKeys> player1, PlayerInput<TKeys> player2)
    {
        Player1 = player1;
        Player2 = player2;
    }

    public PlayerInput<TKeys> Player1 { get; }
    public PlayerInput<TKeys> Player2 { get; }

    public void Update()
    {
        Player1.Update(GetKeys(0));
        Player2.Update(GetKeys(1));
    }

    protected abstract TKeys GetKeys(int player);    
}

