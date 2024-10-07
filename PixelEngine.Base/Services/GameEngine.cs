
using System;

public class GameEngine
{
    public Specs Specs { get; } = new Specs();

    private DiskResourceLoader _diskResourceLoader = new DiskResourceLoader();
    public RenderService RenderService { get; }

    public GameEngine(Func<Specs, BitArrayDataGrid, LayerGroup, Palette, RenderService> createRenderService)
    {
        var patternTable = GensVramImporter.Import(_diskResourceLoader.Load("SampleVRAM/kc.ram"));

        var palette = new Palette(new Color[]
        {
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
        });

        RenderService = createRenderService(Specs, patternTable, new LayerGroup(Specs), palette);
       
        var bg = RenderService.LayerGroup.Background;

        bg.Tiles.ForEach((x, y) =>
        {
            if (x < 8 && y < 8)
            {
                bg.Tiles[x, y] = new Tile(x, y + 64, TileFlags.Normal, PaletteIndex.P0);
            }

            if (x < 4 & y > 16 && y < 20)
            {
                bg.Tiles[x, y] = new Tile(0, y + 80, TileFlags.FlipX, PaletteIndex.P0);
            }
        });

        var fg = RenderService.LayerGroup.Foreground;
        fg.Tiles.ForEach((x, y) =>
        {
            if (x < 8 && y < 8)
            {
                fg.Tiles[x, y] = new Tile(x, y + 64, TileFlags.Normal, PaletteIndex.P0);
            }
        });
    }

    public void Update()
    {
        RenderService.RefreshFrameColors();      
        var bg = RenderService.LayerGroup.Background;
        bg.Scroll.X--;
    }
}


