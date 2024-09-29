
namespace PixelEngine.Diagnostics;

public class FrameRateDisplay
{
    private readonly Stopwatch _elapsedTime;
    private readonly SpriteFont _font;
    private ulong _frameCount;

    public FrameRateDisplay(SpriteFont font)
    {
        _font = font;
        _elapsedTime = new Stopwatch();
        _elapsedTime.Start();
    }

    public void Draw(SpriteBatch spriteBatch, GameWindow window)
    {
        var fps = _frameCount / _elapsedTime.Elapsed.TotalSeconds;

        spriteBatch.Begin(SpriteSortMode.Immediate);
        spriteBatch.DrawString(_font, $"FPS = {fps.ToString("0.0")}", new Vector2(0, window.ClientBounds.Height-16), Color.Red);
        spriteBatch.End();

        _frameCount++;

        if(_frameCount > 100)
        {
            _elapsedTime.Restart();
            _frameCount = 0;
        }
    }
}
