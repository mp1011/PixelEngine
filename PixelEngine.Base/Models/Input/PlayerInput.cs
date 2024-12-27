public abstract class PlayerInput<TKeys> where TKeys:struct
{
    protected TKeys _previous = default, _current = default;


    public abstract bool KeyDown(TKeys keys);
    public abstract bool KeyUp(TKeys keys);

    public void Update(TKeys keys)
    {
        _previous = _current;
        _current = keys;
    }
}

public class GenesisLikePlayerInput : PlayerInput<GenesisPadButtons>
{
    public override bool KeyDown(GenesisPadButtons keys)
    {
        return (_current & keys) == keys;
    }

    public override bool KeyUp(GenesisPadButtons keys)
    {
        return (_current & keys) == 0;
    }
}

