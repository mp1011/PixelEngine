var specs = Specs.GensLike;

var layers = new LayerGroup(
    new ScrollingLayer(specs, 64, 32),
    new ScrollingLayer(specs, 64, 32),
    new Layer(specs, 32, 32),
    new Layer(specs, 4, 4) //todo, sprite layer should be different
    );


var coreRenderService = new RenderService(specs, layers);

Workbench.LoadKcScene(layers, coreRenderService, specs);
// Workbench.SingleTileTest(layers, coreRenderService, specs);

var renderService = new SfmlRenderService(specs, coreRenderService);


while (renderService.WindowIsOpen)
{
   //   layers.Foreground.VScrollTable.AddAll(1);
  //  layers.Background.VScrollTable.AddAll(1);

    renderService.DispatchEvents();
    renderService.DisplayFrame();
}
