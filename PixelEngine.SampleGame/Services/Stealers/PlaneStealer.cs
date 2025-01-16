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

    private Point CalcWorldCameraPosition(int cameraPlaneX, int cameraPlaneY)
    {      
        var newCameraLocation = new Point(cameraPlaneX + (_planeX * _layer.PixelSize.Width), cameraPlaneY + (_planeY * _layer.PixelSize.Height));

        var distance = (newCameraLocation - _camera.WorldLocation).Abs();
        if (_copyEnabled && distance.X > 32)
        {
            var newXLeft = cameraPlaneX + ((_planeX - 1) * _layer.PixelSize.Width);
            var newXRight = cameraPlaneX + ((_planeX + 1) * _layer.PixelSize.Width);

            var distIfLeft = Math.Abs(newXLeft - _camera.WorldLocation.X);
            var distIfRight = Math.Abs(newXRight - _camera.WorldLocation.X);

            if (distIfLeft < distIfRight)
            {
                _planeX--;
                Console.WriteLine($"Plane Dec: {_planeX}");
            }
            else
            {
                _planeX++;
                Console.WriteLine($"Plane Inc: {_planeX}");
            }
            newCameraLocation = new Point(cameraPlaneX + (_planeX * _layer.PixelSize.Width), cameraPlaneY + (_planeY * _layer.PixelSize.Height));
        }

        if (_copyEnabled && distance.Y > 32)
        {
            var newYUp = cameraPlaneX + ((_planeY - 1) * _layer.PixelSize.Height);
            var newYDown = cameraPlaneX + ((_planeY + 1) * _layer.PixelSize.Height);

            var distIfUp = Math.Abs(newYUp - _camera.WorldLocation.Y);
            var distIfDown = Math.Abs(newYDown - _camera.WorldLocation.Y);

            if (distIfUp < distIfDown)
                _planeY--;
            else
                _planeY++;

            newCameraLocation = new Point(cameraPlaneX + (_planeX * _layer.PixelSize.Width), cameraPlaneY + (_planeY * _layer.PixelSize.Height));
        }

        return newCameraLocation;

    }
    public void Update()
    {
        var cameraPlaneX = (-_layer.HScrollTable.Values[0]).NMod(_layer.PixelSize.Width);
        var cameraPlaneY = _layer.VScrollTable.Values[0];
        _camera.WorldLocation = CalcWorldCameraPosition(cameraPlaneX, cameraPlaneY);

        var planeTilePoint = new Point(cameraPlaneX, cameraPlaneY) / _specs.TileSize;
        var worldTilePoint = _camera.WorldLocation / _specs.TileSize;

        var copyOnOff = _copyEnabled ? "ON" : "OFF";
        Debug.Text1 = $"Camera={_camera.WorldLocation.X},{_camera.WorldLocation.Y} WT={worldTilePoint} PT={planeTilePoint})";
        Debug.Text2 = $"Copy {copyOnOff}";

       
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

