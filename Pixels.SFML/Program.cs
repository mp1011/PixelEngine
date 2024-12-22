var specs = Specs.GensLike;
var layers = new LayerGroup(
    new ScrollingLayer(specs, 64, 64),
    new ScrollingLayer(specs, 64, 64),
    new Layer(specs, 32, 32),
    new Layer(specs, 4, 4) //todo, sprite layer should be different
    );


var vram = GensVramImporter.Import(DiskResourceLoader.Load("SampleVRAM\\kc.ram"));
var coreRenderService = new RenderService(specs, layers, vram);
var renderService = new SfmlRenderService(specs, coreRenderService);

GensVramImporter.LoadLayer(DiskResourceLoader.Load("SampleVRAM\\kidc_vram2.ram"), layers.Background, 0xE000);
GensVramImporter.LoadLayer(DiskResourceLoader.Load("SampleVRAM\\kidc_vram2.ram"), layers.Foreground, 0);
GensVramImporter.LoadColors(DiskResourceLoader.Load("SampleVRAM\\colors.ram"), renderService.Palette(0));

layers.Foreground.HScrollTable = new ScrollTable(ScrollTableType.EightPixelStrips, true, specs);
for (short i = 0; i < specs.ScreenHeight/8; i++)
    layers.Foreground.HScrollTable.Values[i] = (short)(i *16);


layers.Background.VScrollTable = new ScrollTable(ScrollTableType.EightPixelStrips, false, specs);
for (short i = 0; i < specs.ScreenWidth/8; i++)
    layers.Background.VScrollTable.Values[i] = (short)(i*8);

coreRenderService.HInterruptCounter = 4;

layers.Foreground.RasterInterupts.Add(new TestRasterInterupt(coreRenderService));

while (renderService.WindowIsOpen)
{
    // layers.Foreground.HScrollTable.AddAll(1);
  //  layers.Background.VScrollTable.AddAll(1);

    renderService.DispatchEvents();
    renderService.DisplayFrame();
}
