public class Workbench
{
    public static void LoadGemTiles(Specs specs)
    {
        //var vrams = DiskResourceLoader.LoadAll("SampleVRAM\\batch")
        //    .Select(p => GensVramImporter.Import(p, specs))
        //    .ToArray();

        //var gemRegions = vrams.Select(p => p.ExtractBlockRegion(0x06EA, 0x06FC, new Size(8, 8)))
        //                     .ToArray();

    }

    public static void SingleTileTest(LayerGroup layers, RenderService coreRenderService, Specs specs)
    {
        coreRenderService.PatternTable.SetData(DiskResourceLoader.Load("SampleVRAM\\kc.ram"));
        GensVramImporter.LoadColors(DiskResourceLoader.Load("SampleVRAM\\colors.ram"), coreRenderService, specs);

      //  layers.Foreground.Resize(new Size(320 / 8, 224 / 8));
         layers.Foreground.Tiles[0, 0] = new Tile(0x00c0, true, false, false, PaletteIndex.P0);      
      //  layers.Foreground.Tiles[0, 0] = new Tile(1, true, false, false, PaletteIndex.P0);
    }

    public static void LoadKcScene(LayerGroup layers, RenderService coreRenderService, Specs specs)
    {
        coreRenderService.PatternTable.SetData(DiskResourceLoader.Load("SampleVRAM\\kc.ram"));
        GensVramImporter.LoadLayer(DiskResourceLoader.Load("SampleVRAM\\kc.ram"), layers.Background, 0xE000);
        GensVramImporter.LoadLayer(DiskResourceLoader.Load("SampleVRAM\\kc.ram"), layers.Foreground, 0);
        GensVramImporter.LoadColors(DiskResourceLoader.Load("SampleVRAM\\colors.ram"), coreRenderService, specs);
        GensVramImporter.LoadSprites(DiskResourceLoader.Load("SampleVRAM\\kc.ram"), 0x1000, specs.NumSprites, coreRenderService.Sprites);

        layers.Foreground.HScrollTable = new ScrollTable(ScrollTableType.FullScreen, true, specs);
        layers.Foreground.VScrollTable = new ScrollTable(ScrollTableType.Line, false, specs);
        layers.Background.HScrollTable = new ScrollTable(ScrollTableType.FullScreen, true, specs);
        layers.Background.VScrollTable = new ScrollTable(ScrollTableType.Line, false, specs);


        layers.Foreground.VScrollTable.SetAll(104);
        layers.Background.VScrollTable.SetAll(8);
    }

}
