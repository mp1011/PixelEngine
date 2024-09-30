namespace PixelEngine.Models.Graphics.GensLike;

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

    public short Index
    {
        get => (short)(_low | ((_high & 7) << 8));
        private set
        {
            _low = (byte)value;
            _high = (byte)((_high & 248) | (value >> 8));
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

    public Tile(short index, TileFlags flags, PaletteIndex paletteIndex) : this()
    {
        Index = index;
        Flags = flags;
        PaletteIndex = paletteIndex;
    }

    public Tile(int index, TileFlags flags, PaletteIndex paletteIndex) : this((short)index, flags, paletteIndex) { }
}