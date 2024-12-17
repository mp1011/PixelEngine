using System;

RenderWindow window = new RenderWindow(
    new VideoMode(640, 480, 32),
    "TEST",
    Styles.None);

FrameRateDisplay frameRateDisplay = new FrameRateDisplay();
Texture canvas = new Texture(320, 240);
RectangleShape shape = new RectangleShape(new Vector2f(640, 480));
shape.Position = new Vector2f(0, 0);
shape.Texture = canvas;

Palette palette = new Palette();
byte[] pixelBuffer = new byte[320 * 240 * 4];
int i = 0;
UpdatePixels(i, pixelBuffer, palette);

while (window.IsOpen)
{
    DummyWork();
    DummyWork();
    DummyWork();
    DummyWork();


    UpdatePixels(i++, pixelBuffer, palette);

    canvas.Update(pixelBuffer);
    window.Clear();
    window.Draw(shape);
    frameRateDisplay.Draw(window);
    window.Display();
}

void DummyWork()
{
    double foo = 0;
    for (int y = 0; y < 240; y++)
    {
        for (int x = 0; x < 320; x++)
        {
            int index = ((y * 320) + x) * 4;
            foo += index;
        }
    }
}

void UpdatePixels(int i, byte[] pixelBuffer, Palette palette)
{
    var index = 0;
    for (int y = 0; y < 240; y++)
    {
        for (int x = 0; x < 320; x++)
        {
            //   int index = ((y * 320) + x) * 4;           
            if (x == 0 || y == 0 || x == 319 || y == 239)
            {
                Marshal.Copy(palette.ColorPointer(6), pixelBuffer, index, Color.Bytes);
            }
            else
            {
                var ppxi = (int)(palette.Length * (Math.Sin(x / 16.0) + Math.Cos(y / 80.0)));

                ppxi = (ppxi + i) % palette.Length;

                var ppx = palette.ColorPointer(ppxi);
                Marshal.Copy(ppx, pixelBuffer, index, Color.Bytes);
            }
            index += Color.Bytes;

        }
    }

}