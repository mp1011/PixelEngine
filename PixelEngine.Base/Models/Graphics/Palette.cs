public unsafe struct Palette
{
    private Color[] _colors;

    public int Length => _colors.Length;

    public Palette(IEnumerable<Color> colors)
    {
        _colors = colors.ToArray();
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

