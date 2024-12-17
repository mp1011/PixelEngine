
using System.Xml.Schema;

RenderWindow window = new RenderWindow(
    new VideoMode(640, 480, 32),
    "TEST",
    Styles.Resize | Styles.Titlebar | Styles.Close);

GameEngine gameEngine = new GameEngine((s, l, p) => new SFMLRenderService(window, s, l, p));
var renderService = gameEngine.RenderService as SFMLRenderService;
if(renderService == null)
    throw new InvalidCastException();

FrameRateDisplay frameRateDisplay = new();

ColorWithHandle c = new ColorWithHandle(new Color(255, 100, 50));
ColorWithHandle bg1 = new ColorWithHandle(new Color(50, 255, 50));
ColorWithHandle bg2 = new ColorWithHandle(new Color(0,0,0,0));

renderService.Vram.Layers[0].Clear(bg1);
renderService.Vram.Layers[1].Clear(bg2);
renderService.Vram.Layers[2].Clear(bg2);
renderService.Vram.Layers[3].Clear(bg2);

ColorWithHandle[] colors = Enumerable.Range(0, 256)
    .Select(p => new ColorWithHandle(new Color((byte)(p * 4), (byte)(255 - (p*2)), (byte)p)))
    .ToArray();


for (int y = 0; y < 32; y++)
{
    for(int x = 0; x < 32; x++)
    {
        renderService.Vram.Layers[3].SetPixel(x, y, c);
    }
}

FrameRateCalculator frameRateCalculator = new();
gameEngine.Update();

int foo = 0;

while (window.IsOpen)
{
    renderService.Vram.Layers[0].Clear(bg1);
    renderService.Vram.Layers[2].Clear(bg2);

    var l = renderService.Vram.Layers[1];
    for(int y = 0; y < l.Height; y++)
    {
        for(int x =0; x < l.Width;x++)
        {
            var cin = (x + y * (foo / 5.0)) % colors.Length;
            l.SetPixel(x, y, colors[(int)cin]);
        }
    }
    foo++;

    window.Clear();
    gameEngine.Update();
    renderService.Draw(window);
    frameRateDisplay.Draw(window);

    window.Display();

}

