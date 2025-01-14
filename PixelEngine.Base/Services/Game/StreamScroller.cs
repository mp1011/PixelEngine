public class StreamScroller
{
    private Specs _specs;
    private ScrollingLayer _background;
    private ScrollingLayer _foreground;
    private LevelTileMap _levelMap;
    
    private BackgroundScrollLayer[] _backgroundHScrollLayers;
    private BackgroundScrollLayer[] _backgroundVScrollLayers;

    private Camera _camera;
    private MovingSprite _focus;
    private CoordinateTranslator _coordinateTranslator;
    private Point _centerLocation;

    private Point _lastStreamCameraLocation = new Point(0, 0);

    public StreamScroller(ScrollingLayer background, 
        ScrollingLayer foreground,
        LevelTileMap levelMap,
        MovingSprite focus, 
        CoordinateTranslator coordinateTranslator, 
        Camera camera,
        BackgroundScrollLayer[] backgroundHScrollLayers,
        BackgroundScrollLayer[] backgroundVScrollLayers,
        Specs specs)
    {
        _specs = specs;
        _background = background;
        _foreground = foreground;
        _levelMap = levelMap;
        _backgroundHScrollLayers = backgroundHScrollLayers;
        _backgroundVScrollLayers = backgroundVScrollLayers;
        _focus = focus;
        _camera = camera;
        _coordinateTranslator = coordinateTranslator;    
        _centerLocation = new Point(specs.ScreenWidth / 2, specs.ScreenHeight / 2);
        _centerLocation -= new Point(focus.PixelWidth / 2, focus.PixelHeight / 2);
    }

    public void Update()
    {
        // todo, calc this offset
        _camera.WorldLocation = new Point((int)_focus.WorldX - 160, (int)_focus.WorldY - 128);
        KeepCameraInBounds();
        
        _foreground.HScrollTable.SetAll((short)-_camera.WorldLocation.X);
        _foreground.VScrollTable.SetAll((short)_camera.WorldLocation.Y);

        int line = 0;
        short vScroll = 0;
        foreach (var bgScrollLayer in _backgroundVScrollLayers)
        {
            vScroll = (short)(_camera.WorldLocation.Y * bgScrollLayer.ScrollFactor);
            _background.VScrollTable.SetRange(line, bgScrollLayer.Lines, vScroll);
            line += bgScrollLayer.Lines;
        }

        Debug.Text1 = vScroll.ToString();

        line = 0;
        foreach (var bgScrollLayer in _backgroundHScrollLayers)
        {
            int adjustedLine = line - vScroll;
            _background.HScrollTable.SetRange(adjustedLine, bgScrollLayer.Lines, (short)(-_camera.WorldLocation.X * bgScrollLayer.ScrollFactor));
            line += bgScrollLayer.Lines;
        }

        var cameraChangeSinceLastStream = (_camera.WorldLocation - _lastStreamCameraLocation).Abs();
        if(cameraChangeSinceLastStream.X >= _specs.TileSize || cameraChangeSinceLastStream.Y >= _specs.TileSize)
        {
            StreamTiles(_lastStreamCameraLocation, _camera.WorldLocation);
           _lastStreamCameraLocation = _camera.WorldLocation;
        }
    }

    private void KeepCameraInBounds()
    {
        if (_camera.WorldLocation.X < 0)
            _camera.WorldLocation = new Point(0, _camera.WorldLocation.Y);

        if (_camera.WorldLocation.Y < 0)
            _camera.WorldLocation = new Point(_camera.WorldLocation.X, 0);

        int maxX = _levelMap.PixelSize.Width - _specs.ScreenWidth;
        int maxY = _levelMap.PixelSize.Height - _specs.ScreenHeight;

        if (_camera.WorldLocation.X > maxX)
            _camera.WorldLocation = new Point(maxX, _camera.WorldLocation.Y);

        if (_camera.WorldLocation.Y > maxY)
            _camera.WorldLocation = new Point(_camera.WorldLocation.X, maxY);
    }

    private void StreamTiles(Point cameraBefore, Point cameraNow)
    {
        if (cameraNow.X > cameraBefore.X)
        {
            var seamBegin =  _camera.WorldLocation.Add(_specs.ScreenWidth, 0) / _specs.TileSize;
            var seamEnd = seamBegin.Add(3, _specs.ScreenHeight / _specs.TileSize);

            StreamTileRange(seamBegin, seamEnd);
        }
        else if (cameraNow.X < cameraBefore.X)
        {
            var seamBegin = _camera.WorldLocation.Add(-_specs.TileSize * 3, 0) / _specs.TileSize;
            var seamEnd = seamBegin.Add(3, _specs.ScreenHeight / _specs.TileSize);

            StreamTileRange(seamBegin, seamEnd);
        }

        if (cameraNow.Y > cameraBefore.Y)
        {
            var seamBegin = _camera.WorldLocation.Add(0, _specs.ScreenHeight) / _specs.TileSize;
            var seamEnd = seamBegin.Add(_specs.ScreenWidth / _specs.TileSize, 3);

            seamBegin = seamBegin.Add(0, -5).NMod(_foreground.TileSize);

            StreamTileRange(seamBegin, seamEnd);
        }
        else if (cameraNow.Y < cameraBefore.Y)
        {
            var seamBegin = _camera.WorldLocation.Add(0, -(_specs.TileSize*3)) / _specs.TileSize;
            var seamEnd = seamBegin.Add(_specs.ScreenWidth / _specs.TileSize, 3);

            StreamTileRange(seamBegin, seamEnd);
        }
    }

    private void StreamTileRange(Point worldBegin, Point worldEnd)
       {
        _levelMap.Tiles.ForEach(worldBegin, worldEnd, (x, y) =>
        {
            var worldPos = new Point(x, y) * _specs.TileSize;
            var planePos = _coordinateTranslator.WorldToPlane(worldPos);
            var planeTile = planePos / _specs.TileSize;
            _foreground.Tiles[planeTile.X, planeTile.Y] = _foreground.Tiles[planeTile].CopyFrom(_levelMap.Tiles[x,y]);
        });
    }

    public void RefreshEntireScreen()
    {
        var begin = _camera.WorldLocation / _specs.TileSize;
        var end = _camera.WorldLocation.Add(_specs.ScreenWidth, _specs.ScreenHeight) / _specs.TileSize;
        //begin = begin.Add(-1, -1).NMod(_foreground.TileSize);
        //end = end.Add(1, 1).NMod(_foreground.TileSize);

        StreamTileRange(begin, end);
    }

    public void RefreshEntireScreen(Point worldOrigin)
    {
        Point planePoint = _coordinateTranslator.ScreenToPlane(new Point(0, 0));
        Point planeCornerTile = planePoint / _specs.TileSize;

        for (int y = 0; y < (_specs.ScreenHeight / _specs.TileSize)+1; y++)
        {
            for (int x = 0; x < (_specs.ScreenWidth / _specs.TileSize)+1; x++)
            {
                var planeTile = planeCornerTile.Add(x, y).NMod(_background.TileSize);                
                var worldTile = worldOrigin.Add(x,y).NMod(_levelMap.Tiles.Size);
                _foreground.Tiles[planeTile.X, planeTile.Y] = _foreground.Tiles[planeTile].CopyFrom(_levelMap.Tiles[worldTile]);
            }
        }
    }

    public override string ToString()
    {
       var cornerPlanePoint = _coordinateTranslator.ScreenToPlane(new Point(0, 0));
       var cornerWorldPoint = _coordinateTranslator.ScreenToWorld(new Point(0, 0));

        return $"Camera={_camera.WorldLocation.X},{_camera.WorldLocation.Y} Plane Scroll={_foreground.HScrollTable.Values[0]},{_foreground.VScrollTable.Values[0]} CPP={cornerPlanePoint} CWP={cornerWorldPoint}";
    }
}

