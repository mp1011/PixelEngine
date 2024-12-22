using System;
using System.Reflection;

public class RenderService
{
    private readonly Specs _specs;
    private readonly LayerGroup _layers;
    private readonly Palette[] _palettes;
    private byte[] _pixelBuffer;
    private BitArrayDataGrid _vram;

   
    public RenderService(Specs specs, LayerGroup layers, BitArrayDataGrid vram)
    {
        _specs = specs;
        _vram = vram;
        _pixelBuffer = new byte[specs.ScreenWidth * specs.ScreenHeight * Color.Bytes];
        _layers = layers;

        var p0 = new Palette(
        [
            new Color(32,96,224),
            new Color(0,0,0),
            new Color(32,32,0),
            new Color(96,96,64),
            new Color(128,128,96),
            new Color(192, 192,160),
            new Color(192, 128,96),
            new Color(64, 0,0),
            new Color(0, 96,0),
            new Color(0, 160,64),
            new Color(64, 224,64),
            new Color(96, 32,0),
            new Color(128, 64,32),
            new Color(160, 96,64),
            new Color(128, 64,96),
            new Color(255, 255,255),
        ]);
        _palettes =
       [
            p0,
             new Palette(Enumerable.Range(0, 64).Select(p => new Color((byte)(p * 4), 0, 0))),
             new Palette(Enumerable.Range(0, 64).Select(p => new Color(0, (byte)(p * 4), 0)))
        ];
    }

    private double dummy = 0;
    public byte[] CalculateFramePixels()
    {
        CalculateLayerPixels(_layers.Background, true);
        CalculateLayerPixels(_layers.Foreground, false);

        return _pixelBuffer;
    }

    private void CalculateLayerPixels(Layer layer, bool isBase)
    {
        int bufferIndex = 0;
        int tileX = 0, tileY = 0, pixelX = 0, pixelY = 0, pixelIndex = 0;
        int srcX = 0, srcY = 0;
        byte colorValue = 0;
        Tile tile = new();

        var scrollX = layer.Scroll.X;
        var scrollY = layer.Scroll.Y;
        var pixelsWidth = layer.PixelSize.Width;
        var pixelsHeight = layer.PixelSize.Height;
        var tileSize = _specs.TileSize;

        for (int y = 0; y < _specs.ScreenHeight; y++)
        {
            for (int x = 0; x < _specs.ScreenWidth; x++)
            {
                srcX = (x + scrollX) % pixelsWidth;
                srcY = (y + scrollY) % pixelsHeight;
                
                tile = layer.Tiles[srcX / 8, srcY / 8];
                if (tile.Index != 0)
                {
                    tileX = tile.Index % _specs.PatternTableTilesAcross;
                    tileY = tile.Index / _specs.PatternTableTilesAcross;

                    if (tile.FlipH)
                        pixelX = (tileX * tileSize) + (tileSize - (srcX % tileSize) - 1);
                    else
                        pixelX = (tileX * tileSize) + (srcX % _specs.TileSize);

                    if (tile.FlipV)
                        pixelY = (tileY * tileSize) + (tileSize - (srcY % tileSize) - 1);
                    else
                        pixelY = (tileY * tileSize) + (srcY % tileSize);

                    pixelIndex = (pixelY * _specs.PatternTableTilesAcross * tileSize) + pixelX;

                    colorValue = _vram[pixelIndex];
                }
                else
                {
                    colorValue = 0;
                }


                if (colorValue != 0 || isBase)
                    _palettes[0].WriteColor(colorValue, _pixelBuffer, bufferIndex);

                bufferIndex += Color.Bytes;
            }
        }
    }

    public byte[] CalculateFramePixels_Test()
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

