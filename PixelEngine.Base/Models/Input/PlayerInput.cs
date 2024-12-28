public abstract class PlayerInput<TKeys> where TKeys:struct
{
    private TKeys _previous = default, _current = default;

    protected abstract bool KeyDown(TKeys keys, TKeys check);

    public bool KeyDown(TKeys key) => KeyDown(_current, key);

    public bool KeyPressed(TKeys key)
    {
        return KeyDown(_current, key) && !KeyDown(_previous, key);
    }

    public void Update(TKeys keys)
    {
        _previous = _current;
        _current = keys;
    }
}

public class GenesisLikePlayerInput : PlayerInput<GenesisPadButtons>
{
    protected override bool KeyDown(GenesisPadButtons keys, GenesisPadButtons check)
    {
        return (keys & check) == check;
    }
}

