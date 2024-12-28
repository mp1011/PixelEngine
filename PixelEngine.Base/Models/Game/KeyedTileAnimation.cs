public record KeyedTileAnimation<TKey>(int StartIndex, Dictionary<TKey, TileAnimationFrame[]> Animations) : TileAnimation(StartIndex)
    where TKey : struct
{
    private TKey _currentAnimation;
    public TKey CurrentAnimation
    {
        get => _currentAnimation;
        set
        {
            if (!EqualityComparer<TKey>.Default.Equals(_currentAnimation, value))
            {
                _currentAnimation = value;
                Reset();
            }
        }
    }

    public override TileAnimationFrame CurrentFrame => Animations[CurrentAnimation][CurrentFrameNumber];

    public override int NumFrames => Animations[CurrentAnimation].Length;
}
