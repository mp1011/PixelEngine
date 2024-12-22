public enum ScrollTableType
{
    FullScreen,
    EightPixelStrips,
    Line
}

public class ScrollTable
{
    public ScrollTableType Type { get; }
    public short[] Values { get; }

    public ScrollTable(ScrollTableType type, bool horizontal, Specs specs)
    {
        int lines = horizontal ? specs.ScreenHeight : specs.ScreenWidth;
        Type = type;

        int numValues = type switch
        {
            ScrollTableType.FullScreen => 1,
            ScrollTableType.EightPixelStrips => lines / 8,
            _ => lines
        };

        Values = Enumerable.Repeat((short)0, numValues).ToArray();
    }

    public short ValueForLine(int line)
    {
        return Type switch
        { 
            ScrollTableType.FullScreen => Values[0],
            ScrollTableType.Line => Values[line],
            _ => Values[line/8]
        };
    }

    public void SetAll(short value)
    {
        for(int i = 0; i < Values.Length; i++)
        {
            Values[i] = value;
        }
    }

    public void AddAll(short value)
    {
        for (int i = 0; i < Values.Length; i++)
        {
            Values[i] += value;
        }
    }
}