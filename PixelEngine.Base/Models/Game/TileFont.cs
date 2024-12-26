public class TileFont
{
    private Dictionary<char, int> _tiles = new Dictionary<char, int>();

    public int this[char c] => _tiles[c];

    public TileFont AddChar(char c, int tile)
    {
        _tiles[c] = tile;
        return this;
    }

    public TileFont AddChars(string text, int firstTile)
    {
        for (int i = 0; i < text.Length; i++)
            AddChar(text[i], firstTile + i);

        return this;
    }
}

