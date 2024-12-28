public abstract record TileAnimation(int StartIndex)
{
    private int _currentFrame = 0;
    public int CurrentFrameNumber
    {
        get => _currentFrame;
        set
        {
            _currentFrame = value.NMod(NumFrames);
        }
    }

    public abstract TileAnimationFrame CurrentFrame { get; }

    public abstract int NumFrames { get; }

    public int GameFramesRemaining { get; set; }

    public void Reset()
    {
        _currentFrame = 0;
        GameFramesRemaining = 1;
    }
}

public record SimpleTileAnimation(int StartIndex, TileAnimationFrame[] Frames) : TileAnimation(StartIndex)
{
    public override TileAnimationFrame CurrentFrame => Frames[CurrentFrameNumber];

    public override int NumFrames => Frames.Length;

    public SimpleTileAnimation(int StartIndex, IEnumerable<TileAnimationFrame> Frames) : this(StartIndex, Frames.ToArray()) { }
}

public record TileAnimationFrame(int Duration, byte[] Data);