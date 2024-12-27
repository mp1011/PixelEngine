var specs = Specs.GensLike;

var layers = new LayerGroup(
    new ScrollingLayer(specs, 64, 32),
    new ScrollingLayer(specs, 64, 32),
    new Layer(specs, 32, 32),
    new Layer(specs, 4, 4) //todo, sprite layer should be different
    );


var coreRenderService = new RenderService(specs, layers);

Workbench.LoadKcScene(layers, coreRenderService, specs);
var inputManager = new SfmlInputManager<GenesisPadButtons>(
    new GenesisLikePlayerInput(),
    new GenesisLikePlayerInput()
    );

var windowManager = new SfmlWindowManager(inputManager);
var renderService = new SfmlRenderService(specs, windowManager, coreRenderService);

var frameAnimator = new TileAnimator(coreRenderService);
frameAnimator.AddAnimation(Workbench.ExtractTileAnimation("gem",0x6ea, 0x6fd, [6, 6, 6]));
frameAnimator.AddAnimation(Workbench.ExtractTileAnimation("ankh", 0x231, 0x233, [120, 6, 6, 6]));

var font = Workbench.CreateHudFont();
var clockString = Workbench.SetupClockSprites(font, coreRenderService);

var clock = new Clock(2, 55, clockString);
ulong frameNumber = 0;

var waterWaver = new WaterWaver(layers.Background, 128, 200);

clockString.Text = "1:23";

while (windowManager.Window.IsOpen)
{
    inputManager.Update();
    frameAnimator.Update(frameNumber);
    clock.Update(frameNumber);
    waterWaver.Update();

    windowManager.DispatchEvents();


    if(inputManager.Player1.KeyDown(GenesisPadButtons.Left))
    {
        coreRenderService.Sprites[11].HorizontalPos--;
    }
    else if (inputManager.Player1.KeyDown(GenesisPadButtons.Right))
    {
        coreRenderService.Sprites[11].HorizontalPos++;
    }

    renderService.DisplayFrame();
    frameNumber++;
}
