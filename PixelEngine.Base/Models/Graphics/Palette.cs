public class Palette
{
    private readonly Color[] _colors;

    public Color this[int index] => _colors[index];

    public Palette(IEnumerable<Color> colors)
    {
        _colors = colors.ToArray();
    }
}

