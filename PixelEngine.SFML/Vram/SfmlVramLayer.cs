using SFML.Graphics;

class SfmlVramLayer : VramLayer<Texture>
{
    private Texture? _canvas;
    private RectangleShape[] _scanLines;
    private RenderWindow _renderWindow;

    public SfmlVramLayer(RenderWindow renderWindow, int width, int height) : base(width, height)
    {
        _renderWindow = renderWindow;
    }

    protected override Texture InitTexture()
    {
        var canvas = new Texture((uint)Width, (uint)Height);

        // todo, window size in pixels
        _scanLines = new RectangleShape[240];
        for(int i = 0; i < _scanLines.Length; i++)
        {
            _scanLines[i] = new RectangleShape(new Vector2f(Width, 1));
            _scanLines[i].Position = new Vector2f(100, 100 + i);
            _scanLines[i].Texture = canvas;
            _scanLines[i].TextureRect = new IntRect(0, i, Width, 1);
        }
 
        return canvas;
    }

    protected override void ApplyBuffer(byte[] buffer, Texture texture)
    {
        texture.Update(buffer);
    }

    public override void Draw()
    {
        foreach(var s in _scanLines)
            _renderWindow.Draw(s);
    }

}

