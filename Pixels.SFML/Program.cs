var specs = Specs.GensLike;
var layers = new LayerGroup(
    new Layer(specs, 64, 64),
    new Layer(specs, 64, 64),
    new Layer(specs, 32, 32),
    new Layer(specs, 4, 4) //todo, sprite layer should be different
    );


var vram = GensVramImporter.Import(DiskResourceLoader.Load("SampleVRAM\\kc.ram"));
var renderService = new SfmlRenderService(specs, new RenderService(specs, layers, vram));

GensVramImporter.LoadLayer(DiskResourceLoader.Load("SampleVRAM\\kidc_vram2.ram"), layers.Background, 0xE000);
GensVramImporter.LoadLayer(DiskResourceLoader.Load("SampleVRAM\\kidc_vram2.ram"), layers.Foreground, 0);
GensVramImporter.LoadColors(DiskResourceLoader.Load("SampleVRAM\\colors.ram"), renderService.Palette(0));

while (renderService.WindowIsOpen)
{
   // layers.Background.Scroll.X++;
  //  layers.Background.Scroll.Y++;
    renderService.DispatchEvents();
    renderService.DisplayFrame();
}
