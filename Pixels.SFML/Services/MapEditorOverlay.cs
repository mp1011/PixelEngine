interface IDisplayOverlay
{
    void Draw(RenderWindow window);
}
class MapEditorOverlay : IDisplayOverlay
{
    private readonly KcMapEditor _editor;
    private ArrayDataGrid<Shape> _tileOverlays;
    private SfmlWindowManager _windowManager;
    public MapEditorOverlay(KcMapEditor editor, SfmlWindowManager windowManager, Specs specs)
    {
        _editor = editor;
        _windowManager = windowManager;
        _tileOverlays = new ArrayDataGrid<Shape>(specs.ScreenWidth / specs.TileSize, specs.ScreenHeight / specs.TileSize);

        float xScale = windowManager.WindowSize.Width / specs.ScreenWidth;
        float yScale = windowManager.WindowSize.Width / specs.ScreenHeight;

        _tileOverlays.ForEach((x, y) =>
        {
            var tile = new RectangleShape(new Vector2f(specs.TileSize * xScale, specs.TileSize * yScale));
            tile.Position = new Vector2f((float)(x * specs.TileSize * xScale), (float)(y * specs.TileSize * yScale));
           // tile.OutlineColor = SFML.Graphics.Color.Blue;
           // tile.OutlineThickness = 1;
            tile.FillColor = new SFML.Graphics.Color(0, 0, 0, 0);
            _tileOverlays[x, y] = tile;
        });
    }

    private SFML.Graphics.Color TileFillColor(int x, int y)
    {
        switch (_editor.TilePropertiesAt(x, y))
        {
            case TileType.Solid:
                return new SFML.Graphics.Color(0, 0, 255, 100);
            case TileType.Block:
            case TileType.Prize:
                return new SFML.Graphics.Color(0, 255, 0, 100);
            default:
                return new SFML.Graphics.Color(0, 0, 0, 0);
        }
    }

    public void Draw(RenderWindow window)
    {
        _tileOverlays.ForEach((x, y) =>
        {
            var tile = _tileOverlays[x, y];
            tile.FillColor = TileFillColor(x, y);
            window.Draw(tile);
        });
    }
}

