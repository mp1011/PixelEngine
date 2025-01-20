public class FrameRateDisplay : IDisplayOverlay
{
    private readonly Font _font;
    private readonly Text _text;
    private readonly FrameRateCalculator _calculator;

    public FrameRateDisplay()
    {
        _calculator = new FrameRateCalculator();
        _font = new Font("C:\\Windows\\Fonts\\arial.ttf");
        _text = new Text("", _font, 16);
        _text.Position = new Vector2f(0, 400);
    }

    public void Draw(RenderWindow window, float xScale, float yScale)
    {
        var fps = _calculator.CalcFPS();
        _text.DisplayedString = $"FPS = {fps.ToString("0.0")}";
        window.Draw(_text);
    }
}
