public class DebugString
{
    private readonly Font _font;
    private readonly Text _text;

    public string Text { get; set; }

    public DebugString(int y)
    {
        _font = new Font("C:\\Windows\\Fonts\\arial.ttf");
        _text = new Text("", _font, 16);
        _text.Position = new Vector2f(0, y);
        Text = "TEST";
    }

    public void Draw(RenderWindow window)
    {
        _text.DisplayedString = Text;
        window.Draw(_text);
    }
}
