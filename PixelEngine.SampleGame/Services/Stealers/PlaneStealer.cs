using System.ComponentModel;
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
    private LevelTileMap _worldMap;
    private bool _copyEnabled = false;
        
    public PlaneStealer(ScrollingLayer layer, InputManager inputManager, Specs specs)
    {
        _specs = specs;
        _layer = layer;
        _inputManager = inputManager;
        _camera = new Camera();
        _worldMap = new LevelTileMap(new Size(1000, 1000), _specs);
        _coordinateTranslator = new CoordinateTranslator(_layer, _worldMap, _camera, _specs);

    }

    public void Update()
    {
        var cameraPlaneX = (-_layer.HScrollTable.Values[0]).NMod(_layer.PixelSize.Width);
        var cameraPlaneY = _layer.VScrollTable.Values[0];

        _camera.WorldLocation = new Point(cameraPlaneX + (_planeX * _layer.PixelSize.Width), cameraPlaneY + (_planeY * _layer.PixelSize.Height));

        var planeTilePoint = new Point(cameraPlaneX, cameraPlaneY) / _specs.TileSize;
        var worldTilePoint = _camera.WorldLocation / _specs.TileSize;

        var copyOnOff = _copyEnabled ? "ON" : "OFF";
        Debug.Text1 = $"Camera={_camera.WorldLocation.X},{_camera.WorldLocation.Y} WT={worldTilePoint} PT={planeTilePoint})";
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

        _worldMap.Tiles.ForEach((x, y) =>
        {
            if (x > maxX && _worldMap.Tiles[x, y].Index > 0)
                maxX = x;
            if (y > maxY && _worldMap.Tiles[x, y].Index > 0)
                maxY = y;
        });

        var trimmedMap = new LevelTileMap(new Size(maxX + 1, maxY + 1), _specs);
        trimmedMap.Tiles.CopyFrom(
            _worldMap.Tiles, 
            new Rectangle(0, 0, trimmedMap.Tiles.Width-1, trimmedMap.Tiles.Height-1), 
            new Point(0, 0),
            trimmedMap.SetFromOtherMap);

        byte[] buffer = new byte[4 + trimmedMap.Tiles.Length * 2];

        buffer[0] = (byte)((trimmedMap.Tiles.Width & 0xFF00) >> 8);
        buffer[1] = (byte)(trimmedMap.Tiles.Width & 255);
        buffer[2] = (byte)((trimmedMap.Tiles.Height & 0xFF00) >> 8);
        buffer[3] = (byte)(trimmedMap.Tiles.Height & 255);

        int index = 4;
        foreach(var tile in trimmedMap.Tiles.ToArray())
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
        _worldMap.Tiles.ForEach(worldCorner, worldCorner.Add(_specs.ScreenWidth / _specs.TileSize, _specs.ScreenHeight / _specs.TileSize),
            (x, y) =>
            {
                var relX = x - worldCorner.X;
                var relY = y - worldCorner.Y;

                var worldTile = _worldMap.Tiles[x, y];
                var planeTile = _layer.Tiles[(planeCorner.X + relX) % _layer.TileSize.Width, (planeCorner.Y + relY) % _layer.TileSize.Height];

                if (worldTile.Index == 0)
                    _worldMap.SetFromOtherMap(x,y,planeTile);
            });
    }
}

