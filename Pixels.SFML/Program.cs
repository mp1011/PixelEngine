var specs = Specs.GensLike;


var coreRenderService = new RenderService(specs);

//var engine = new StaticEngine("test.gs0", "kc.cram", coreRenderService, specs);
var engine = new StaticEngine("headdy.gs0", "headdy.cram", coreRenderService, specs);
//var engine = new StaticEngine("ecco.gs0", "ecco.cram", coreRenderService, specs);

engine.Load();

var inputManager = new SfmlInputManager(
    new PlayerInput(),
    new PlayerInput()
    );

var windowManager = new SfmlWindowManager(inputManager);
var renderService = new SfmlRenderService(specs, windowManager, coreRenderService);

ulong frameNumber = 0;
while (windowManager.Window.IsOpen)
{
    inputManager.Update();
    engine.Update(frameNumber++); 
    windowManager.DispatchEvents();
    renderService.DebugString = Debug.Text1;
    renderService.DisplayFrame();
}
