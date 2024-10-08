GameEngine gameEngine = new GameEngine((s, b, l, p) => new SFMLRenderService(s, b, l, p));
var renderService = gameEngine.RenderService as SFMLRenderService;
if(renderService == null)
    throw new InvalidCastException();

FrameRateDisplay frameRateDisplay = new();

RenderWindow window = new RenderWindow(
    new VideoMode(640,480,32), 
    "TEST", 
    Styles.Resize | Styles.Titlebar | Styles.Close);

FrameRateCalculator frameRateCalculator = new();
double fps = 0;
while(window.IsOpen)
{
    window.Clear();
    gameEngine.Update();
    renderService.Draw(window);
    frameRateDisplay.Draw(window);

    window.Display();

}