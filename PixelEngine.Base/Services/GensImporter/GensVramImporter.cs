public class GensVramImporter
{
    public static void LoadColors(byte[] data, RenderService renderService, Specs specs)
    {
        var bitReader = new BitStreamReader(data);
        for (int i = 0; i < specs.NumPalettes; i++)
        {
            var palette = renderService.Palette(i);
            LoadColors(bitReader, palette, specs);
        }
    }

    private static void LoadColors(BitStreamReader bitReader, Palette palette, Specs specs)
    {
        for (int colorIndex = 0; colorIndex < specs.ColorsPerPalette; colorIndex++)
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
