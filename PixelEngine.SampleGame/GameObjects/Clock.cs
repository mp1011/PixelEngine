public class Clock
{
    private readonly SpriteString _text;
    private byte _minutes, _seconds;
    
    public Clock(byte minutes, byte seconds, SpriteString text)
    {
        _text = text;
        _minutes = minutes;
        _seconds = seconds;
    }

    public void Update(ulong frameNumber)
    {
        if ((frameNumber % 60) != 0)
            return;

        if(_seconds == 0)
        {
            _minutes--;
            _seconds = 59;
        }
        else
        {
            _seconds--;
        }

        _text.Text = $"{_minutes}:{_seconds.ToString("00")}";
    }
}