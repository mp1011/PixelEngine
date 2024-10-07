// See https://aka.ms/new-console-template for more information
using SkiaSharp;

Console.WriteLine("Hello, World!");

// define the surface properties
var info = new SKImageInfo(256, 256);

// construct a new surface
var surface = SKSurface.Create(info);

// get the canvas from the surface
var canvas = surface.Canvas;
canvas.DrawPoint(new SKPoint(16, 16), new SKColor(180, 10, 10));