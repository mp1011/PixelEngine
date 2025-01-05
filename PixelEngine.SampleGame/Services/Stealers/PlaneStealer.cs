class PlaneStealer
{
    private Specs _specs;
    private InputManager _inputManager;
    private ScrollingLayer _layer;
    private CoordinateTranslator _coordinateTranslator;
    private Camera _camera;
    private int _planeX = 0, _planeY = 0;
    private ArrayDataGrid<Tile> _worldMap;
        
    public PlaneStealer(ScrollingLayer layer, InputManager inputManager, Specs specs)
    {
        _specs = specs;
        _layer = layer;
        _inputManager = inputManager;
        _camera = new Camera();
        _coordinateTranslator = new CoordinateTranslator(_layer, _camera);
        _worldMap = new ArrayDataGrid<Tile>(1000, 1000);
    }

    public void Update()
    {
        var cameraPlaneX = (-_layer.HScrollTable.Values[0]).NMod(_layer.PixelSize.Width);
        var cameraPlaneY = _layer.VScrollTable.Values[0];

        _camera.WorldLocation = new Point(cameraPlaneX + (_planeX * _layer.PixelSize.Width), cameraPlaneY + (_planeY * _layer.PixelSize.Height));

        var planeTilePoint = new Point(cameraPlaneX, cameraPlaneY) / _specs.TileSize;
        var worldTilePoint = _camera.WorldLocation / _specs.TileSize;


        Debug.Text1 = $"Camera = {_camera.WorldLocation.X},{_camera.WorldLocation.Y} (Tile {worldTilePoint.X},{worldTilePoint.Y})";


        if (_inputManager.Player1.KeyPressed(GamepadButtons.Right))
            _planeX++;
        else if (_inputManager.Player1.KeyPressed(GamepadButtons.Left))
            _planeX--;

        if (_inputManager.Player1.KeyPressed(GamepadButtons.Up))
            _planeY--;
        else if (_inputManager.Player1.KeyPressed(GamepadButtons.Down))
            _planeY++;

        CopyScreenTiles(planeTilePoint, worldTilePoint);
    }

    private void CopyScreenTiles(Point planeCorner, Point worldCorner)
    {
        _worldMap.ForEach(worldCorner, worldCorner.Add(_layer.TileSize.Width, _layer.TileSize.Height),
            (x, y) =>
            {
                var relX = x - worldCorner.X;
                var relY = y - worldCorner.Y;

                var worldTile = _worldMap[x, y];
                var planeTile = _layer.Tiles[(planeCorner.X + relX) % _layer.TileSize.Width, (planeCorner.Y + relY) % _layer.TileSize.Height];

                if (worldTile.Index == 0)
                    worldTile.CopyFrom(planeTile);
            });
    }
}

