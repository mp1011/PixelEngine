public class GensVramImporter
{
    public static BitArrayDataGrid Import(byte[] vramData)
    {
        var tiles = new BitArrayDataGrid(128, 1000, new NBitArray(4, 128 * 1000));
        for(int currentTile = 0; currentTile < 2000;  currentTile++)
        {
            var tileBytes = vramData
                .Skip(currentTile * 32)
                .Take(32)
                .ToArray();

            SetTileData(tiles, tileBytes, currentTile);
        }

        return tiles;
    }

    public static void LoadLayer(byte[] vramData, Layer layer, int vramIndex)
    {
        for(int i = 0; i < layer.Tiles.Length; i++)
        {
            layer.Tiles[i] = new Tile(vramData[vramIndex + 1], vramData[vramIndex]);
            if (layer.Tiles[i].Index >= 2000)
            {
                // not sure what these would be
                layer.Tiles[i] = new Tile(0, 0);
            }
            vramIndex += 2;
        }
    }

    public static void LoadColors(byte[] data, Palette palette)
    {
        var bitReader = new BitStreamReader(data);
        for(int colorIndex = 0; colorIndex < 16; colorIndex++)
        {
            bitReader.ReadNextBits(1);

            byte r = bitReader.ReadNextBits(3);
            bitReader.ReadNextBits(1);

            byte g = bitReader.ReadNextBits(3);
            bitReader.ReadNextBits(1);

            byte b = bitReader.ReadNextBits(3);
            bitReader.ReadNextBits(4);

            r = (byte)(r * 36);
            g = (byte)(g * 36);
            b = (byte)(b * 36);

            palette.SetColor(colorIndex, new Color(r,g,b));
        }
    }

    private static void SetTileData(BitArrayDataGrid tiles, byte[] tileBytes, int currentTile)
    {
        NBitArray tile = new NBitArray(4, 64);
        tile.SetData(tileBytes);
        BitArrayDataGrid sourceTile = new BitArrayDataGrid(8, 8, tile);

        var currentTileX = currentTile % 16;
        var currentTileY = currentTile / 16;

        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                int destX = (currentTileX * 8) + x;
                int destY = (currentTileY * 8) + y;

                int srcX = 3 - x;
                int srcY = y;

                if(x >= 4)
                {
                    srcX = 4 + (3 - (x-4));
                }
                
                tiles[destX,destY] = sourceTile[srcX,srcY];
            }
        }
    }

    private static void SwapBytes(byte[] vramData)
    {
        for(int i = 0; i < vramData.Length; i += 2)
        {
            var b0 = vramData[i];
            vramData[i] = vramData[i + 1];
            vramData[i + 1] = b0;
        }
    }
}
