var specs = Specs.GensLike;

var inputManager = new SfmlInputManager(
    new PlayerInput(),
    new PlayerInput()
    );

var coreRenderService = new RenderService(specs);

var engine = new StealerEngine(StealerMode.PlaneExtractor, MemoryLocations.Local, coreRenderService, inputManager, specs);
//var engine = new KcMockup(coreRenderService, inputManager, specs);

engine.Load();


var windowManager = new SfmlWindowManager(inputManager);
var renderService = new SfmlRenderService(specs, windowManager, coreRenderService);

ulong frameNumber = 0;
while (windowManager.Window.IsOpen)
{
    inputManager.Update();
    engine.Update(frameNumber++); 
    windowManager.DispatchEvents();
    renderService.DebugString1 = Debug.Text1;
    renderService.DebugString2 = Debug.Text2;
    renderService.DisplayFrame();
}
