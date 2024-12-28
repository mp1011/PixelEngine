public class TileAnimator
{
    private List<TileAnimation> _animations = new List<TileAnimation>();
    private List<int> _animationFrames = new List<int>();
    private RenderService _renderService;

    public TileAnimator(RenderService renderService)
    {
        _renderService = renderService;
    }

    public void AddAnimation(TileAnimation animation)
    {
        _animations.Add(animation);
        animation.Reset();
        WriteTiles(animation);
    }

    public void Update(ulong frameNumber)
    {
        foreach(var animation in _animations)
            Update(animation);        
    }

    private void Update(TileAnimation tileAnimation)
    {
        if(--tileAnimation.GameFramesRemaining <= 0)
        {
            tileAnimation.CurrentFrameNumber++;
            tileAnimation.GameFramesRemaining = (int)(tileAnimation.CurrentFrame.Duration * tileAnimation.DurationScale);
            WriteTiles(tileAnimation);
        }
    }

    private void WriteTiles(TileAnimation tileAnimation)
    {
        _renderService.PatternTable.Write(tileAnimation.StartIndex, tileAnimation.CurrentFrame.Data);
    }
}

