public class RenderService
{
    private readonly Specs _specs;
    private readonly LayerGroup _layers;
    private readonly Palette[] _palettes;
    private byte[] _pixelBuffer;
    private int _hInterruptLinesRemaining;

    public byte HInterruptCounter { get; set; }
   
    public Palette Palette(int index) => _palettes[index];

    public Sprite[] Sprites { get; }

    public PatternTable PatternTable { get; }

    public RenderService(Specs specs, LayerGroup layers)
    {
        _specs = specs;
        PatternTable = new PatternTable(specs);
        _pixelBuffer = new byte[specs.ScreenWidth * specs.ScreenHeight * Color.Bytes];
        _layers = layers;

        _palettes = Enumerable.Range(0, specs.NumPalettes)
                              .Select(p => new Palette(specs.ColorsPerPalette))
                              .ToArray();

        Sprites = Enumerable.Range(0, specs.NumSprites)
                             .Select(p => new Sprite())
                             .ToArray();
    }

    public byte[] CalculateFramePixels()
    {
        Array.Clear(_pixelBuffer,0, _pixelBuffer.Length);
        _hInterruptLinesRemaining = HInterruptCounter;

        for (int y = 0; y < _specs.ScreenHeight; y++)
            DrawScanline(y);

        DrawSprites();

        return _pixelBuffer;
    }

    private void DrawScanline(int y)
    {
        int pixelsPerTile = _specs.TileSize * _specs.TileSize;
        int bufferIndex = 0;
        int tileX = 0, tileY = 0, pixelX = 0, pixelY = 0, pixelIndex = 0, srcX = 0, srcY = 0, tileStart = 0;
        byte colorValue = 0;
        Tile tile = new();

        short bgScrollX = 0, bgScrollY = 0, fgScrollX=0, fgScrollY = 0;
        var bgPixelsWidth = _layers.Background.PixelSize.Width;
        var bgPixelsHeight = _layers.Background.PixelSize.Height;
        var fgPixelsWidth = _layers.Foreground.PixelSize.Width;
        var fgPixelsHeight = _layers.Foreground.PixelSize.Height;
        var tileSize = _specs.TileSize;

        var fg = _layers.Foreground;
        var bg = _layers.Background;

        bool drewFgPixel = false;

        
        bufferIndex = y * _specs.ScreenWidth * Color.Bytes;

        bgScrollX = bg.HScrollTable.ValueForLine(y);
        fgScrollX = fg.HScrollTable.ValueForLine(y);

        for (int x = 0; x < _specs.ScreenWidth; x++)
        {
            bgScrollY = bg.VScrollTable.ValueForLine(x);
            fgScrollY = fg.VScrollTable.ValueForLine(x);

            drewFgPixel = false;
            // these two blocks are near copies
            #region FG
            srcX = (x + fgScrollX) % fgPixelsWidth;
            srcY = (y + fgScrollY) % fgPixelsHeight;

            tile = fg.Tiles[srcX / tileSize, srcY / tileSize];
            if (tile.Index != 0)
            {
                tileStart = tile.Index * pixelsPerTile;

                tileX = srcX - ((srcX / tileSize) * tileSize);
                tileY = srcY - ((srcY / tileSize) * tileSize);

                if (tile.FlipH)
                    tileX = (tileSize - tileX - 1);

                if (tile.FlipV)
                    tileY = (tileSize - tileY - 1);

                colorValue = PatternTable.TilePixel(tile.Index, tileX, tileY); ;
            }
            else
            {
                colorValue = 0;
            }

            if (colorValue != 0)
            {
                _palettes[(int)tile.PaletteIndex].WriteColor(colorValue, _pixelBuffer, bufferIndex);
                drewFgPixel = true;
            }
            #endregion

            #region BG
            if (!drewFgPixel)
            {
                srcX = (x + bgScrollX) % bgPixelsWidth;
                srcY = (y + bgScrollY) % bgPixelsHeight;

                tile = bg.Tiles[srcX / tileSize, srcY / tileSize];
                if (tile.Index != 0)
                {
                    tileStart = tile.Index * pixelsPerTile;

                    tileX = srcX - ((srcX / tileSize) * tileSize);
                    tileY = srcY - ((srcY / tileSize) * tileSize);

                    if (tile.FlipH)
                        tileX = (tileSize - tileX - 1);

                    if (tile.FlipV)
                        tileY = (tileSize - tileY - 1);

                    colorValue = PatternTable.TilePixel(tile.Index, tileX, tileY); ;
                }
                else
                {
                    colorValue = 0;
                }

                if (colorValue != 0)
                    _palettes[(int)tile.PaletteIndex].WriteColor(colorValue, _pixelBuffer, bufferIndex);
                else
                    _palettes[0].WriteColor(colorValue, _pixelBuffer, bufferIndex);
            }
            #endregion

            bufferIndex += Color.Bytes;
        }
       
        if (HInterruptCounter > 0)
        {
            if (_hInterruptLinesRemaining == 0)
            {
                _hInterruptLinesRemaining = HInterruptCounter;
                foreach (var interupt in bg.RasterInterupts)
                {
                    interupt.OnHBlank(bg, y);
                }
                foreach (var interupt in bg.RasterInterupts)
                {
                    interupt.OnHBlank(bg, y);
                }
            }

            _hInterruptLinesRemaining--;
        }

    }

    private void DrawSprites()
    {
        foreach(var sprite in OrderedSprites())
        {
            DrawSprite(sprite);
        }
    }

    private IEnumerable<Sprite> OrderedSprites()
    {
        List<Sprite> sprites = new List<Sprite>();

        var nextSprite = Sprites[0];
        sprites.Add(nextSprite);

        while (nextSprite.Next != 0)
        {
            nextSprite = Sprites[nextSprite.Next];
            sprites.Add(nextSprite);
        }

        sprites.Reverse();
        return sprites;   
    }

    private bool DrawSprite(Sprite sprite)
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
        bool drewSprite = false;

        for (int x = 0; x < pixelsWide; x++)
        {
            if ((x % tileSize) == 0)
            {
                var column = x / tileSize;

                if (sprite.HorizontalFlip)
                    column = sprite.HSize - column;

                columnTileBegin = (column * (sprite.VSize + 1));

                if (sprite.VerticalFlip)
                    columnTileBegin += (sprite.VSize);
            }

            tileNumber = columnTileBegin;

            for (int y = 0; y < pixelsTall; y++)
            {
                screenX = (sprite.HorizontalPos - 128) + x;
                screenY = (sprite.VerticalPos - 128) + y;

                if(y > 0 && (y % tileSize) == 0)
                {
                    if (sprite.VerticalFlip)
                        tileNumber--;
                    else
                        tileNumber++;
                }
      
                if (screenX < 0 || screenX >= _specs.ScreenWidth)
                    continue;
                if (screenY < 0 || screenY >= _specs.ScreenHeight)
                    continue;

                bufferIndex = ((screenY * _specs.ScreenWidth) + screenX) * Color.Bytes;

                if (sprite.Priority || PixelAvailableForLowPrioritySprite(screenX, screenY))
                {
                    //todo, flip
                    pixelX = x % tileSize;
                    pixelY = y % tileSize;

                    if(sprite.HorizontalFlip)
                        pixelX = tileSize - pixelX - 1;

                    if (sprite.VerticalFlip)
                        pixelY = tileSize - pixelY - 1;

                    colorValue = PatternTable.TilePixel(sprite.Tile + tileNumber, pixelX, pixelY);

                    if (colorValue > 0)
                        _palettes[sprite.PaletteIndex].WriteColor(colorValue, _pixelBuffer, bufferIndex);
                }
            }
        }

        return drewSprite;
    }

    private bool PixelAvailableForLowPrioritySprite(int screenX, int screenY)
    {
        var bgScrollV = _layers.Background.VScrollTable.ValueForLine(screenX);
        var bgScrollH = _layers.Background.HScrollTable.ValueForLine(screenY);
        var fgScrollV = _layers.Foreground.VScrollTable.ValueForLine(screenX);
        var fgScrollH = _layers.Foreground.HScrollTable.ValueForLine(screenY);

        var bgX = (screenX + bgScrollH) % (_layers.Background.PixelSize.Width);
        var bgY = (screenY + bgScrollV) % (_layers.Background.PixelSize.Height);
        var fgX = (screenX + fgScrollH) % (_layers.Foreground.PixelSize.Width);
        var fgY = (screenY + fgScrollV) % (_layers.Foreground.PixelSize.Height);

        var bgTile = _layers.Background.Tiles[bgX/ _specs.TileSize, bgY / _specs.TileSize];
        var fgTile = _layers.Foreground.Tiles[fgX / _specs.TileSize, fgY / _specs.TileSize];

        var tileSize = _specs.TileSize;
        if (bgTile.Priority)
        {
            var tileX = bgX - ((bgX / tileSize) * tileSize);
            var tileY = bgY - ((bgY / tileSize) * tileSize);

            if (bgTile.FlipH)
                tileX = (tileSize - tileX - 1);

            if (bgTile.FlipV)
                tileY = (tileSize - tileY - 1);

            if (PatternTable.TilePixel(bgTile.Index, tileX, tileY) != 0)
                return false;
        }


        if(fgTile.Priority)
        {
            var tileX = fgX - ((fgX / tileSize) * tileSize);
            var tileY = fgY - ((fgY / tileSize) * tileSize);

            if (fgTile.FlipH)
                tileX = (tileSize - tileX - 1);

            if (fgTile.FlipV)
                tileY = (tileSize - tileY - 1);

            if (PatternTable.TilePixel(fgTile.Index, tileX, tileY) != 0)
                return false;
        }

        return true;
    }
}

