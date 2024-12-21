public class RenderService
{
    private readonly Specs _specs;
    private readonly LayerGroup _layers;
    private readonly Palette[] _palettes;
    private byte[] _pixelBuffer;
   
    public RenderService(Specs specs, LayerGroup layers)
    {
        _specs = specs;
        _pixelBuffer = new byte[specs.ScreenWidth * specs.ScreenHeight * Color.Bytes];
        _layers = layers;

        _palettes =
       [
             new Palette(Enumerable.Range(0, 64).Select(p => new Color((byte)(p * 4), 0, 0))),
             new Palette(Enumerable.Range(0, 64).Select(p => new Color(0, (byte)(p * 4), 0)))
        ];    
    }

    private double dummy = 0;
    public byte[] CalculateFramePixels()
    {
        dummy += 0.1;
        int paletteIndex = 0;

        var index = 0;
        for (int y = 0; y < _specs.ScreenHeight; y++)
        {
            for (int x = 0; x < _specs.ScreenWidth; x++)
            {
                paletteIndex = y % _palettes.Length;

                if (x == 0 || y == 0 || x == _specs.ScreenWidth-1 || y == _specs.ScreenHeight-1)
                {
                    _palettes[paletteIndex].WriteColor(6, _pixelBuffer, index);
                }
                else
                {
                    var ppxi = (int)(_palettes[paletteIndex].Length * (Math.Sin((x + dummy) / (16.0 * 1+dummy)) + Math.Cos((y - dummy) / 80.0)));
                    ppxi = Math.Abs(ppxi) % _palettes[paletteIndex].Length;
                    _palettes[paletteIndex].WriteColor(ppxi, _pixelBuffer, index);
                }
                index += Color.Bytes;
            }
        }

        return _pixelBuffer;
    }
}

