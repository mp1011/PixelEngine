var specs = Specs.GensLike;
var layers = new LayerGroup(
    new ScrollingLayer(specs, 64, 32),
    new ScrollingLayer(specs, 64, 32),
    new Layer(specs, 32, 32),
    new Layer(specs, 4, 4) //todo, sprite layer should be different
    );


var vram = GensVramImporter.Import(DiskResourceLoader.Load("SampleVRAM\\kc.ram"));
var coreRenderService = new RenderService(specs, layers, vram);
var renderService = new SfmlRenderService(specs, coreRenderService);

GensVramImporter.LoadLayer(DiskResourceLoader.Load("SampleVRAM\\kc.ram"), layers.Background, 0xE000);
GensVramImporter.LoadLayer(DiskResourceLoader.Load("SampleVRAM\\kc.ram"), layers.Foreground, 0);
GensVramImporter.LoadColors(DiskResourceLoader.Load("SampleVRAM\\colors.ram"), coreRenderService, specs);
GensVramImporter.LoadSprites(DiskResourceLoader.Load("SampleVRAM\\kc.ram"), 0x1000, specs.NumSprites, coreRenderService.Sprites);

layers.Foreground.HScrollTable = new ScrollTable(ScrollTableType.FullScreen, true, specs);
layers.Foreground.VScrollTable = new ScrollTable(ScrollTableType.Line, false, specs);
layers.Background.HScrollTable = new ScrollTable(ScrollTableType.FullScreen, true, specs);
layers.Background.VScrollTable = new ScrollTable(ScrollTableType.Line, false, specs);


layers.Foreground.VScrollTable.SetAll(104);
layers.Background.VScrollTable.SetAll(0);

while (renderService.WindowIsOpen)
{
   //  layers.Foreground.VScrollTable.AddAll(1);
  //  layers.Background.VScrollTable.AddAll(1);

    renderService.DispatchEvents();
    renderService.DisplayFrame();
}
