public enum ScrollTableType
{
    FullScreen,
    EightPixelStrips,
    Line
}

public class ScrollTable
{
    private readonly short _limit;
    public ScrollTableType Type { get; }
    public short[] Values { get; }

    public ScrollTable(ScrollTableType type, bool horizontal, int limit, Specs specs)
    {
        int lines = horizontal ? specs.ScreenHeight : specs.ScreenWidth;
        Type = type;

        _limit = (short)limit;

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

    public void Set(int index, short value)
    {
        Values[index] = (short)(value.NMod((short)_limit));
    }

    public void Add(int index, short value)
    {
        index = index.NMod(Values.Length);
        Values[index] = (short)(Values[index] + value).NMod(_limit);
    }

    public void SetAll(short value)
    {
        for(int i = 0; i < Values.Length; i++)
        {
            Values[i] = value.NMod(_limit);
        }
    }

    public void SetRange(int start, int count, short value)
    {
        int index = start;
        while(count-- > 0)
        {
            if (index >= 0 && index < Values.Length)
            {
                Values[index] = value.NMod(_limit);
            }
            index++;
        }
    }

    public void AddAll(short value)
    {
        for (int i = 0; i < Values.Length; i++)
        {
            Values[i] = (short)(Values[i] + value).NMod(_limit);
        }
    }
}