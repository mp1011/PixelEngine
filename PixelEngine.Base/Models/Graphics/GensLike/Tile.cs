public enum PaletteIndex
{
    P0,
    P1,
    P2,
    P3
}

public struct Tile
{
    private byte _low, _high;

    public short Index
    {
        get => (short)(_low | ((_high & 7) << 8));
        private set
        {
            _low = (byte)value;
            _high = (byte)((_high & 248) | (value >> 8));
        }
    }

    public bool Priority
    {
        get => (_high & 0x80) != 0;
        set
        {
            _high = (byte)((_high & 0x7f) | (value ? 1 : 0) << 7);
        }
    }

    public bool FlipH
    {
        get => (_high & 0x8) != 0;
        set
        {
            _high = (byte)((_high & 0xf7) | (value ? 1 : 0) << 3);
        }
    }

    public bool FlipV
    {
        get => (_high & 0x10) != 0;
        set
        {
            _high = (byte)((_high & 0xef) | (value ? 1 : 0) << 4);
        }
    }

    public PaletteIndex PaletteIndex
    {
        get => (PaletteIndex)((_high & 0x60) >> 5);
        set
        {
            _high = (byte)((_high & 0x9f) | ((byte)value << 5));
        }
    }

    public Tile(byte high, byte low)
    {
        _high = high;
        _low = low;
    }

    public Tile(short index, bool priority, bool flipH, bool flipV, PaletteIndex paletteIndex) : this()
    {
        Index = index;
        FlipH = flipH;
        FlipV = flipV;
        Priority = priority;
        PaletteIndex = paletteIndex;
    }

    public override string ToString()
    {
        var p = Priority ? " Priority" : "";
        var h = FlipH ? " FlipH" : "";
        var v = FlipV ? " FlipH" : "";

        return $"{Index} {PaletteIndex}{p}{h}{v}";
    }

    public void WriteBytes(byte[] buffer, int index)
    {
        buffer[index] = _high;
        buffer[index + 1] = _low;
    }
}