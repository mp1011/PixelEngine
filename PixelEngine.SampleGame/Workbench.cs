using System.Runtime.Intrinsics.Arm;

public class Workbench
{
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

        layers.Foreground.HScrollTable = new ScrollTable(ScrollTableType.Line, true, specs);
        layers.Foreground.VScrollTable = new ScrollTable(ScrollTableType.FullScreen, false, specs);
        layers.Background.HScrollTable = new ScrollTable(ScrollTableType.Line, true, specs);
        layers.Background.VScrollTable = new ScrollTable(ScrollTableType.FullScreen, false, specs);



        layers.Foreground.VScrollTable.SetAll(104);
        layers.Background.VScrollTable.SetAll(8);
    }

    public static TileAnimation ExtractTileAnimation(string subFolder, int tileStart, int tileEnd, int[] durations)
    {
        int startIndex = tileStart * 0x20;
        int length = (tileEnd - tileStart) * 0x20;

        var vrams = DiskResourceLoader.LoadAll($"SampleVRAM\\batch\\{subFolder}")
            .Select(p => p.Skip(startIndex).Take(length).ToArray())
            .ToArray();

        return new TileAnimation(startIndex, vrams.Select((data,ix) => new TileAnimationFrame(durations[ix], data)));
    }

    public static TileFont CreateHudFont()
    {
        return new TileFont().AddChars("0123456789:x", 0x6ba);
    }

    public static SpriteString SetupClockSprites(TileFont font, RenderService renderService)
    {
        return new SpriteString(font, "2:55", 3, renderService.Sprites);
    }

}
