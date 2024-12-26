public record TileAnimation(int StartIndex, TileAnimationFrame[] Frames)
{
    private int _currentFrame = 0;
    public int CurrentFrameNumber
    {
        get => _currentFrame;
        set
        {
            _currentFrame = value % Frames.Length;
        }
    }

    public TileAnimationFrame CurrentFrame => Frames[_currentFrame];

    public int GameFramesRemaining { get; set; }

    public TileAnimation(int StartIndex, IEnumerable<TileAnimationFrame> Frames) : this(StartIndex, Frames.ToArray()) { }

    public void Reset()
    {
        _currentFrame = 0;
        GameFramesRemaining = CurrentFrame.Duration;
    }
}

public record TileAnimationFrame(int Duration, byte[] Data);