public class PatternTable
{
    private readonly Specs _specs;
    private byte[] _data;

    public PatternTable(Specs specs)
    {
        _specs = specs;
        _data = new byte[8];
    }

    public void Write(int index,  byte[] data)
    {
        Array.Copy(data, 0, _data, index, data.Length);
    }

    public byte TilePixel(int tile, int x, int y)
    {
        var tileStart = tile * 32;
        var lineStart = tileStart + (y * 4);

        //ugly, fix me
        return x switch
        {
            0 => (byte)((_data[lineStart + 1] & 240) >> 4),
            1 => (byte)(_data[lineStart + 1] & 15),
            2 => (byte)((_data[lineStart] & 240) >> 4),
            3 => (byte)(_data[lineStart] & 15),
            4 => (byte)((_data[lineStart + 3] & 240) >> 4),
            5 => (byte)(_data[lineStart + 3] & 15),
            6 => (byte)((_data[lineStart + 2] & 240) >> 4),
            7 => (byte)(_data[lineStart + 2] & 15),
            _ => 0
        };
    }

    public void SetData(byte[] bytes)
    {
        _data = bytes;
    }
}
