

var specs = Specs.GensLike;

var inputManager = new SfmlInputManager(
    new PlayerInput(),
    new PlayerInput()
    );

var coreRenderService = new RenderService(specs);

//var engine = new StealerEngine(StealerMode.PlaneExtractor, MemoryLocations.FromDisk(), coreRenderService, inputManager, specs);
//var engine = new StreamScrollerTester("kc.ram", "map.bin", coreRenderService, inputManager, specs);
//var engine = new KcMapEditor("kc.ram", "map.bin", "tile_props.bin", coreRenderService, inputManager, specs);
var engine = new KcMockup("kc.ram", "map.bin", "tile_props.bin", coreRenderService, inputManager, specs);

engine.Load();

//var planeViewer = new SfmlDiagnosticWindowManager();
var windowManager = new SfmlWindowManager(inputManager, specs);
var renderService = new SfmlRenderService(specs, windowManager, coreRenderService);
//var diagnosticRenderService = new DiagnosticRenderService(coreRenderService.Layers.Foreground.PixelSize, coreRenderService, specs, planeViewer, engine.CoordinateTranslator);

var debugString1 = new DebugString(320);
var debugString2 = new DebugString(340);

renderService.AddOverlay(new FrameRateDisplay());
renderService.AddOverlay(debugString1);
renderService.AddOverlay(debugString2);
renderService.AddOverlay(new CollidersOverlay());



//renderService.AddOverlay(new MapEditorOverlay(engine, windowManager, specs));


ulong frameNumber = 0;
while (windowManager.Window.IsOpen)
{
    inputManager.Update();
    engine.Update(frameNumber++); 
    
    windowManager.DispatchEvents();
  //  planeViewer.DispatchEvents();

    debugString1.Text = Debug.Text1;
    debugString2.Text = Debug.Text2;
    renderService.DisplayFrame();
   //wessssssssssssss45e33333333333333 diagnosticRenderService.DrawPlane(coreRenderService.Layers.Foreground, planeViewer.Window);
}
