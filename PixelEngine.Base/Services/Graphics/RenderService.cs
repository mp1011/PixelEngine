public class RenderService
{
    private readonly Specs _specs;
    private readonly LayerGroup _layers;
    private readonly Palette[] _palettes;
    private byte[] _pixelBuffer;
    private BitArrayDataGrid _vram; 
    private int _hInterruptLinesRemaining;

    public byte HInterruptCounter { get; set; }
   
    public Palette Palette(int index) => _palettes[index];

    public Sprite[] Sprites { get; }

    public RenderService(Specs specs, LayerGroup layers, BitArrayDataGrid vram)
    {
        _specs = specs;
        _vram = vram;
        _pixelBuffer = new byte[specs.ScreenWidth * specs.ScreenHeight * Color.Bytes];
        _layers = layers;

        _palettes = Enumerable.Range(0, specs.NumPalettes)
                              .Select(p => new Palette(specs.ColorsPerPalette))
                              .ToArray();

        Sprites = Enumerable.Range(0, specs.NumSprites)
                             .Select(p => new Sprite())
                             .ToArray();
    }

    private double dummy = 0;
    public byte[] CalculateFramePixels()
    {
        CalculateScrollingLayerPixels(_layers.Background);
        CalculateScrollingLayerPixels(_layers.Foreground);

        DrawSprites();
        return _pixelBuffer;
    }

    private void DrawSprites()
    {
        for(int i = 0; i < _specs.NumSprites; i++)
        {
            DrawSprite(Sprites[i]);
        }
    }

    private void DrawSprite(Sprite sprite)
    {
        int pixelsWide = (sprite.HSize + 1) * _specs.TileSize;
        int pixelsTall = (sprite.VSize + 1) * _specs.TileSize;
        int bufferIndex = 0;
        int screenX;
        int screenY;
        int tileX, tileY;
        int pixelX, pixelY;
        int tileSize = _specs.TileSize;

        byte colorValue = 0;
        int pixelIndex = 0;

        int tileNumber = 0;
        int columnTileBegin = 0;

        for (int x = 0; x < pixelsWide; x++)
        {
            if (x > 0 && (x % tileSize) == 0)
                columnTileBegin += sprite.VSize + 1;

            tileNumber = columnTileBegin;

            for (int y = 0; y < pixelsTall; y++)
            {
                screenX = (sprite.HorizontalPos - 128) + x;
                screenY = (sprite.VerticalPos - 128) + y;

                if(y > 0 && (y % tileSize) == 0)
                {
                    tileNumber++;
                }
      
                if (screenX < 0 || screenX >= _specs.ScreenWidth)
                    continue;
                if (screenY < 0 || screenY >= _specs.ScreenHeight)
                    continue;

                bufferIndex = ((screenY * _specs.ScreenWidth) + screenX) * Color.Bytes;


                tileX = (sprite.Tile + tileNumber) % _specs.PatternTableTilesAcross;
                tileY = (sprite.Tile + tileNumber) / _specs.PatternTableTilesAcross;

                if (sprite.HorizontalFlip)
                    pixelX = (tileX * tileSize) + (tileSize - (x % tileSize) - 1);
                else
                    pixelX = (tileX * tileSize) + (x % _specs.TileSize);

                if (sprite.VerticalFlip)
                    pixelY = (tileY * tileSize) + (tileSize - (y % tileSize) - 1);
                else
                    pixelY = (tileY * tileSize) + (y % tileSize);

                pixelIndex = (pixelY * _specs.PatternTableTilesAcross * tileSize) + pixelX;

                colorValue = _vram[pixelIndex];

                if (colorValue > 0)
                {
                    _palettes[sprite.PaletteIndex].WriteColor(colorValue, _pixelBuffer, bufferIndex);
                }
            }
        }
    }

    private void CalculateScrollingLayerPixels(ScrollingLayer layer)
    {
        int bufferIndex = 0;
        int tileX = 0, tileY = 0, pixelX = 0, pixelY = 0, pixelIndex = 0;
        int srcX = 0, srcY = 0;
        byte colorValue = 0;
        Tile tile = new();

        short scrollX = 0;
        short scrollY = 0;
        var pixelsWidth = layer.PixelSize.Width;
        var pixelsHeight = layer.PixelSize.Height;
        var tileSize = _specs.TileSize;

        _hInterruptLinesRemaining = HInterruptCounter;

        for (int y = 0; y < _specs.ScreenHeight; y++)
        {
            scrollX = layer.HScrollTable.ValueForLine(y);

            for (int x = 0; x < _specs.ScreenWidth; x++)
            {
                scrollY = layer.VScrollTable.ValueForLine(x);
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


                if (colorValue != 0)
                    _palettes[(int)tile.PaletteIndex].WriteColor(colorValue, _pixelBuffer, bufferIndex);


                bufferIndex += Color.Bytes;
            }

            if (HInterruptCounter > 0)
            {
                if (_hInterruptLinesRemaining == 0)
                {
                    _hInterruptLinesRemaining = HInterruptCounter;
                    foreach (var interupt in layer.RasterInterupts)
                    {
                        interupt.OnHBlank(layer, y);
                    }
                }

                _hInterruptLinesRemaining--;
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

