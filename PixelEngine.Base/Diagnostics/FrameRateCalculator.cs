public class FrameRateCalculator
{
    private readonly Stopwatch _elapsedTime;
    private ulong _frameCount;

    public FrameRateCalculator()
    {
        _elapsedTime = new Stopwatch();
        _elapsedTime.Start();
    }

    public double CalcFPS()
    {
        var fps = _frameCount / _elapsedTime.Elapsed.TotalSeconds;
        _frameCount++;

        if(_frameCount > 100)
        {
            _elapsedTime.Restart();
            _frameCount = 0;
        }
        return fps;
    }
}
