public class SpriteString
{
    private Sprite[] _sprites;
    private RenderService _renderService;
    private readonly TileFont _font;
    private readonly int _spriteIndex;

    private string _text = "";
    public string Text
    {
        get => _text;
        set
        {
            _text = value;

            var spriteIndex = _spriteIndex;
            for(int i = 0; i < value.Length; i++) 
            {
                _sprites[spriteIndex].Tile = _font[value[i]];
                spriteIndex = _sprites[spriteIndex].Next;
            }
        }
    }

    public void Foo(ref Sprite sprite)
    {

    }

    public SpriteString(TileFont font, string text, int spriteIndex, Sprite[] sprites)
    {
        _spriteIndex = spriteIndex;
        _sprites = sprites;
        _font = font;
        Text = text;
    }
}