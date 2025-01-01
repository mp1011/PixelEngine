public enum ScrollTableType
{
    FullScreen,
    EightPixelStrips,
    Line
}

public class ScrollTable
{
    private readonly ushort _limit;
    public ScrollTableType Type { get; }
    public ushort[] Values { get; }

    public ScrollTable(ScrollTableType type, bool horizontal, int limit, Specs specs)
    {
        int lines = horizontal ? specs.ScreenHeight : specs.ScreenWidth;
        Type = type;

        _limit = (ushort)limit;

        int numValues = type switch
        {
            ScrollTableType.FullScreen => 1,
            ScrollTableType.EightPixelStrips => lines / 8,
            _ => lines
        };

        Values = Enumerable.Repeat((ushort)0, numValues).ToArray();
    }

    public ushort ValueForLine(int line)
    {
        return Type switch
        { 
            ScrollTableType.FullScreen => Values[0],
            ScrollTableType.Line => Values[line],
            _ => Values[line/8]
        };
    }

    public void Set(int index, short value)
    {
        Values[index] = (ushort)(value.NMod((short)_limit));
    }

    public void SetAll(ushort value)
    {
        for(int i = 0; i < Values.Length; i++)
        {
            Values[i] = value;
            Values[i] = (ushort)(Values[i] % _limit);
        }
    }

    public void SetAll(short value)
    {
        for (int i = 0; i < Values.Length; i++)
        {
            Values[i] = (ushort)(value.NMod((short)_limit));
        }
    }

    public void SetRange(int start, int count, ushort value)
    {
        int index = start;
        while(count-- > 0)
        {
            if (index >= 0 && index < Values.Length)
            {
                Values[index] = value;
                Values[index] = (ushort)(Values[index] % _limit);
            }
            index++;
        }
    }

    public void AddAll(ushort value)
    {
        for (int i = 0; i < Values.Length; i++)
        {
            Values[i] += value;
            Values[i] = (ushort)(Values[i] % _limit);
        }
    }
}