public class FrameRateDisplay
{
    private readonly FrameRateCalculator _calculator;
    private readonly Font _font;
    private readonly Text _text;

    public FrameRateDisplay()
    {
        _calculator = new FrameRateCalculator();
         
        _font = new Font("C:\\Windows\\Fonts\\arial.ttf");
        _text = new Text("", _font, 16);
        _text.Position = new Vector2f(0, 400);
    }

    public void Draw(RenderWindow window)
    {
        var fps = _calculator.CalcFPS();
        _text.DisplayedString = $"FPS = {fps.ToString("0.0")}";
        window.Draw(_text);
    }
}
