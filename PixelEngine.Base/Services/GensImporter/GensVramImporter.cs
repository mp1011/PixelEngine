public class GensVramImporter
{
    public static void LoadLayer(byte[] vramData, Layer layer, int vramIndex)
    {
        for (int i = 0; i < layer.Tiles.Length; i++)
        {
            layer.Tiles[i] = new Tile(vramData[vramIndex + 1], vramData[vramIndex]);
            vramIndex += 2;
        }
    }

    public static void LoadSprites(byte[] vramData, int vramIndex, int numSprites, Sprite[] destination)
    {
        for (int i = 0; i < numSprites; i++)
        {
            destination[i] = new Sprite(vramData, vramIndex);
            vramIndex += 8;
        }
    }

    public static void LoadColors(byte[] data, RenderService renderService, Specs specs)
    {
        var bitReader = new BitStreamReader(data);
        for (int i = 0; i < specs.NumPalettes; i++)
        {
            var palette = renderService.Palette(i);
            LoadColors(bitReader, palette);
        }
    }

    private static void LoadColors(BitStreamReader bitReader, Palette palette)
    {
        for (int colorIndex = 0; colorIndex < 16; colorIndex++)
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

            palette.SetColor(colorIndex, new Color(r, g, b));
        }
    }
}   
