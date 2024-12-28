public class PlayerInput
{
    private GamepadButtons _previous = default, _current = default;

    protected bool KeyDown(GamepadButtons keys, GamepadButtons check)
    {
        return (keys & check) == check;
    }

    public bool KeyDown(GamepadButtons key) => KeyDown(_current, key);

    public bool KeyPressed(GamepadButtons key)
    {
        return KeyDown(_current, key) && !KeyDown(_previous, key);
    }

    public void Update(GamepadButtons keys)
    {
        _previous = _current;
        _current = keys;
    }
}

