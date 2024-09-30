namespace PixelEngine.Extensions;

public static class PointExtensions
{
    public static Point DivideBy(this Point point, int divisor)
    {
        return new Point(point.X / divisor, point.Y / divisor);
    }
}
