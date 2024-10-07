using SkiaSharp.Views.UWP;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI;
using System.Xml.Schema;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PixelEngine.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private CanvasDevice canvasDevice;
        private CanvasRenderTarget renderTarget;

        SKBitmap _drawBuffer = new SKBitmap(320, 240, SKColorType.Rgb888x, SKAlphaType.Unknown);
        SKCanvas _bufferCanvas;
        private Stopwatch _timer;
        private long _lastDrawTime;

        public MainPage()
        {
            this.InitializeComponent();
            _bufferCanvas = new SKCanvas(_drawBuffer);
            _timer = new Stopwatch();
            _timer.Start();
            CompositionTarget.Rendering += OnFrameRendering;
        }

        void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            for (int y = 0; y < 320; y++)
            {
                for (int x = 0; x < 240; x++)
                {
                    args.DrawingSession.DrawRectangle(new Rect(x * 2, y * 2, 2, 2),
                        Color.FromArgb(255, (byte)(f % 256), (byte)((x + f) % 256), (byte)((y + f) % 256)));
                }
            }
            f++;

            //args.DrawingSession.DrawEllipse(155, 115, 80, 30, Colors.Black, 3);
            //args.DrawingSession.DrawText("Hello, world!", 100, 100, Colors.Yellow);
        }

        private void OnFrameRendering(object sender, object e)
        {
            if(_timer.ElapsedMilliseconds > _lastDrawTime + 16)
            {
                _lastDrawTime = _timer.ElapsedMilliseconds;
                drawCanvas.Invalidate();
             //   DrawTo(_bufferCanvas);
             // skiaCanvas.Invalidate();
            }
        }

        private double FPS()
        {
            return f / (_timer.ElapsedMilliseconds / 1000.0);
        }
       
        private int f = 0;
        private void DrawTo(SKCanvas canvas)
        {
            // Clear the canvas
            canvas.Clear(SKColors.Black);

            using (var paint = new SKPaint())
            {
                // Set paint color to black
                paint.Color = SKColors.Black;

                for (int yy = 0; yy < 640; yy++)
                {
                    for (int xx = 0; xx < 480; xx++)
                    {
                        var x = xx / 2;
                        var y = yy / 2;
                        paint.Color = new SKColor((byte)(f % 256), (byte)((x + f) % 256), (byte)((y + f) % 256));
                        canvas.DrawPoint(x, y, paint); // Draw point at each grid cell
                    }
                }


                canvas.DrawText(FPS().ToString("0.0"), new SKPoint(16, 16), paint);
            }


            f++;
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            DrawTo(e.Surface.Canvas);
        }
    }
}
