

var specs = Specs.GensLike;

var inputManager = new SfmlInputManager(
    new PlayerInput(),
    new PlayerInput()
    );

var coreRenderService = new RenderService(specs);

//var engine = new StealerEngine(StealerMode.PlaneExtractor, MemoryLocations.Local, coreRenderService, inputManager, specs);
var engine = new StreamScrollerTester("kc.ram", "map.bin", coreRenderService, inputManager, specs);

//var engine = new KcMockup(coreRenderService, inputManager, specs);

engine.Load();

var planeViewer = new SfmlDiagnosticWindowManager();
var windowManager = new SfmlWindowManager(inputManager);
var renderService = new SfmlRenderService(specs, windowManager, coreRenderService);
var diagnosticRenderService = new DiagnosticRenderService(coreRenderService.Layers.Foreground.PixelSize, coreRenderService, specs);

ulong frameNumber = 0;
while (windowManager.Window.IsOpen)
{
    inputManager.Update();
    engine.Update(frameNumber++); 
    
    windowManager.DispatchEvents();
    planeViewer.DispatchEvents();

    renderService.DebugString1 = Debug.Text1;
    renderService.DebugString2 = Debug.Text2;
    renderService.DisplayFrame();
    diagnosticRenderService.DrawPlane(coreRenderService.Layers.Foreground, planeViewer.Window);
}
