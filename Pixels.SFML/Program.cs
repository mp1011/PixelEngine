var specs = Specs.GensLike;
var layers = new LayerGroup(
    new Layer(specs, 64, 64),
    new Layer(specs, 64, 64),
    new Layer(specs, 32, 32),
    new Layer(specs, 4, 4) //todo, sprite layer should be different
    );
var renderService = new SfmlRenderService(specs, new RenderService(specs, layers));


layers.Background.Tiles.ForEach((x, y) =>
{
    layers.Background.Tiles[0, 0] = new Tile(16 * 12, false, false, false, PaletteIndex.P0);
    layers.Background.Tiles[1, 0] = new Tile((16 * 12)+1, false, false, false, PaletteIndex.P0);
    layers.Background.Tiles[0, 1] = new Tile((16 * 12) + 1, false, false, false, PaletteIndex.P0);

    layers.Background.Tiles[0, 2] = new Tile(16 * 12, false, true, false, PaletteIndex.P0);
    layers.Background.Tiles[0, 3] = new Tile(16 * 12, false, false, true, PaletteIndex.P0);
    layers.Background.Tiles[0, 4] = new Tile(16 * 12, false, true, true, PaletteIndex.P0);

    if (x > 8 && y > 8)
        layers.Background.Tiles[x, y] = new Tile(16*14, false, false, false, PaletteIndex.P0);
});


layers.Foreground.Tiles.ForEach((x, y) =>
{
    if ((x % 3) == 0 && (y % 3) == 0)
        layers.Foreground.Tiles[x, y] = new Tile(16 * 15, false, false, false, PaletteIndex.P0);
});

while (renderService.WindowIsOpen)
{
    layers.Foreground.Scroll.X++;
    layers.Background.Scroll.Y--;
    renderService.DispatchEvents();
    renderService.DisplayFrame();
}
