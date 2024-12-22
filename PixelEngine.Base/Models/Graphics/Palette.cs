public unsafe struct Palette
{
    private Color[] _colors;

    public int Length => _colors.Length;

    public Palette(IEnumerable<Color> colors)
    {
        _colors = colors.ToArray();
    }

    public void SetColor(int index, Color color)
    {
        _colors[index] = color;
    }

    public Palette(int numColors)
    {
        _colors = Enumerable.Range(0, numColors).Select(p => new Color(0, 0, 0)).ToArray();
    }

    public Color this[int index] => _colors[index];

    public void WriteColor(int index, byte[] buffer, int bufferIndex)
    {
        fixed(Color* c0 = &_colors[0])
        {
            Color* ptr = c0 + index;
            Marshal.Copy((nint)ptr, buffer, bufferIndex, Color.Bytes);
        }
    }
}

