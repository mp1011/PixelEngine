public enum TileFlags
{
    Normal = 0,
    Priority = 1,
    FlipX = 2,
    FlipY = 4
}

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

    //public short Index
    //{
    //    get => (short)(_low | ((_high & 7) << 8));
    //    private set
    //    {
    //        _low = (byte)value;
    //        _high = (byte)((_high & 248) | (value >> 8));
    //    }
    //}

    //0-15
    public short X
    {
        get => (short)(_low & 15);
        private set
        {
            _low = (byte)((_low & 240) | (byte)value);
        }
    }

    //0-127
    public short Y
    {
        get => (short)((_high & 7) << 4 | ((_low & 240) >> 4));
        private set
        {
            _low = (byte)((_low & 15) | (byte)value << 4);
            _high = (byte)((_high & 248) | value >> 4);
        }
    }

    public TileFlags Flags
    {
        get => (TileFlags)((_high & 224) >> 5);
        set
        {
            _high = (byte)((_high & 31) | ((byte)value << 5));
        }
    }

    public PaletteIndex PaletteIndex
    {
        get => (PaletteIndex)((_high & 24) >> 3);
        set
        {
            _high = (byte)((_high & 231) | ((byte)value << 3));
        }
    }

    public Tile(short x, short y, TileFlags flags, PaletteIndex paletteIndex) : this()
    {
        X = x;
        Y = y;
        Flags = flags;
        PaletteIndex = paletteIndex;
    }

    public Tile(int x, int y, TileFlags flags, PaletteIndex paletteIndex) : this((short)x, (short)y, flags, paletteIndex) { }
}