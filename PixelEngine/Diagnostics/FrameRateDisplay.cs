public class FrameRateDisplay
{
    private readonly SpriteFont _font;
    private readonly FrameRateCalculator _calculator;

    public FrameRateDisplay(SpriteFont font)
    {
        _font = font;
        _calculator = new FrameRateCalculator();
    }

    public void Draw(SpriteBatch spriteBatch, GameWindow window)
    {
        var fps = _calculator.CalcFPS();

        spriteBatch.Begin(SpriteSortMode.Immediate);
        spriteBatch.DrawString(_font, $"FPS = {fps.ToString("0.0")}", new Vector2(0, window.ClientBounds.Height - 16), XnaColor.Red);
        spriteBatch.End();
    }
}
