var specs = Specs.GensLike;

var inputManager = new SfmlInputManager(
    new PlayerInput(),
    new PlayerInput()
    );

var coreRenderService = new RenderService(specs);

//var engine = new StaticEngine("test.gs0", "kc.cram", coreRenderService, inputManager, specs);
//var engine = new StaticEngine("headdy.gs0", "headdy.cram", coreRenderService, inputManager, specs);
//var engine = new StaticEngine("sor.gs0", "sor.cram", coreRenderService, inputManager, specs);
//var engine = new StaticEngine("sonic.gs0", "sonic.cram", coreRenderService, inputManager, specs);
//var engine = new StaticEngine("ecco.gs0", "ecco.cram", coreRenderService, inputManager, specs);
var engine = new KcMockup(coreRenderService, inputManager, specs);

engine.Load();


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
