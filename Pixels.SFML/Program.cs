var specs = Specs.GensLike;
var layers = new LayerGroup(
    new Layer(specs, 64, 64),
    new Layer(specs, 64, 64),
    new Layer(specs, 32, 32),
    new Layer(specs, 4, 4) //todo, sprite layer should be different
    );
var renderService = new SfmlRenderService(specs, new RenderService(specs, layers));


while (renderService.WindowIsOpen)
{
    renderService.DispatchEvents();
    renderService.DisplayFrame();
}
