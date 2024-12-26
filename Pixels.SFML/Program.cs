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

var frameAnimator = new TileAnimator(coreRenderService);
frameAnimator.AddAnimation(Workbench.ExtractTileAnimation("gem",0x6ea, 0x6fd, [6, 6, 6]));
frameAnimator.AddAnimation(Workbench.ExtractTileAnimation("ankh", 0x231, 0x233, [120, 6, 6, 6]));

var font = Workbench.CreateHudFont();
var clockString = Workbench.SetupClockSprites(font, coreRenderService);

var clock = new Clock(2, 55, clockString);
ulong frameNumber = 0;

var waterWaver = new WaterWaver(layers.Background, 128, 200);

clockString.Text = "1:23";

while (renderService.WindowIsOpen)
{
    frameAnimator.Update(frameNumber);
    clock.Update(frameNumber);
    waterWaver.Update();

    renderService.DispatchEvents();
    renderService.DisplayFrame();
    frameNumber++;
}
