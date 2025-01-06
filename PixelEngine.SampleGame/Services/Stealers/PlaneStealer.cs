using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

class PlaneStealer
{
    private Specs _specs;
    private InputManager _inputManager;
    private ScrollingLayer _layer;
    private CoordinateTranslator _coordinateTranslator;
    private Camera _camera;
    private int _planeX = 0, _planeY = 0;
    private ArrayDataGrid<Tile> _worldMap;
    private bool _copyEnabled = false;
        
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

        var copyOnOff = _copyEnabled ? "ON" : "OFF";
        Debug.Text1 = $"Camera = {_camera.WorldLocation.X},{_camera.WorldLocation.Y} (Tile {worldTilePoint.X},{worldTilePoint.Y})";
        Debug.Text2 = $"Copy {copyOnOff}";


        if (_inputManager.Player1.KeyPressed(GamepadButtons.Right))
            _planeX++;
        else if (_inputManager.Player1.KeyPressed(GamepadButtons.Left))
            _planeX--;

        if (_inputManager.Player1.KeyPressed(GamepadButtons.Up))
            _planeY--;
        else if (_inputManager.Player1.KeyPressed(GamepadButtons.Down))
            _planeY++;

        if (_inputManager.Player1.KeyPressed(GamepadButtons.A))
            _copyEnabled = !_copyEnabled;

        if (_inputManager.Player1.KeyPressed(GamepadButtons.B))
            SaveWorldMap();

        if (_copyEnabled)
            CopyScreenTiles(planeTilePoint, worldTilePoint);        
    }

    private void SaveWorldMap()
    {
        int maxX = 0;
        int maxY = 0;

        _worldMap.ForEach((x, y) =>
        {
            if (x > maxX && _worldMap[x, y].Index > 0)
                maxX = x;
            if (y > maxY && _worldMap[x, y].Index > 0)
                maxY = y;
        });

        var trimmedMap = new ArrayDataGrid<Tile>(maxX + 1, maxY + 1);
        trimmedMap.CopyFrom(_worldMap, new Rectangle(0, 0, trimmedMap.Width-1, trimmedMap.Height-1), new Point(0, 0));

        byte[] buffer = new byte[trimmedMap.Length * 2];
        int index = 0;
        foreach(var tile in trimmedMap.ToArray())
        {
            tile.WriteBytes(buffer, index);
            index += 2;
        }
        File.WriteAllBytes("map.bin", buffer);
    }

    private bool InCaptureRange(int worldX, int worldY)
    {
        int minX = _planeX * _layer.TileSize.Width;
        int minY = _planeY * _layer.TileSize.Height;

        int maxX = minX + _layer.TileSize.Width;
        int maxY = minY + _layer.TileSize.Height;

        return worldX >= minX && worldX <= maxX && worldY >= minY && worldY <= maxY;
    }

    private void CopyScreenTiles(Point planeCorner, Point worldCorner)
    {
        _worldMap.ForEach(worldCorner, worldCorner.Add(_layer.TileSize.Width, _layer.TileSize.Height),
            (x, y) =>
            {
                if (!InCaptureRange(x, y))
                    return;

                var relX = x - worldCorner.X;
                var relY = y - worldCorner.Y;

                var worldTile = _worldMap[x, y];
                var planeTile = _layer.Tiles[(planeCorner.X + relX) % _layer.TileSize.Width, (planeCorner.Y + relY) % _layer.TileSize.Height];

                if (worldTile.Index == 0)
                    _worldMap[x,y] = planeTile;
            });
    }
}

