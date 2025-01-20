public enum TileType : byte
{
    Empty,
    Solid,
    Block,
    Prize,
    Max=Prize
}

public class TileProperties
{
    private readonly Dictionary<short, TileType> _tileTypes = new Dictionary<short, TileType>();

    public TileType this[short index]
    {
        get => _tileTypes.GetValueOrDefault(index);
        set => _tileTypes[index] = value;
    }
        
    public TileProperties(string filename)
    {
        var buffer = DiskResourceLoader.Load($"MapData\\{filename}");
        int index = 0;
        while(index < buffer.Length)
        {
            short tileNumber = (short)((buffer[index] << 8) | buffer[index + 1]);
            TileType tileType = (TileType)buffer[index + 2];
            if (tileType > TileType.Max)
                tileType = TileType.Empty;
            _tileTypes[tileNumber] = tileType;
            index += 3;
        }
    }
    
    public void WriteToDisk()
    {
        byte[] buffer = new byte[_tileTypes.Count * 3];
        int index = 0;
        foreach(var kvp in _tileTypes)
        {
            buffer[index] = (byte)((kvp.Key & 0xFF00) >> 8);
            buffer[index + 1] = (byte)(kvp.Key & 0xFF);
            buffer[index + 2] = (byte)kvp.Value;
            index += 3;
        }
        File.WriteAllBytes("tile_props.bin", buffer);
    }
}