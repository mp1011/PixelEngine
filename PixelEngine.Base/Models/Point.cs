public record Point(int X, int Y)
{
    public Point Add(int addX, int addY) => new Point(X +addX, Y + addY);

    public static Point operator -(Point p1, Point p2)
    {
        return new Point(p1.X - p2.X, p1.Y - p2.Y);
    }

    public static Point operator +(Point p1, Point p2)
    {
        return new Point(p1.X + p2.X, p1.Y + p2.Y);
    }

    public static Point operator /(Point p1, int value)
    {
        return new Point(p1.X / value, p1.Y / value);
    }

    public static Point operator *(Point p1, int value)
    {
        return new Point(p1.X * value, p1.Y * value);
    }

    public override string ToString() => $"{X},{Y}";
    public Point NMod(Size size) => new Point(X.NMod(size.Width), Y.NMod(size.Height));
    public Point NMod(int value) => new Point(X.NMod(value), Y.NMod(value));

    public Point Abs() => new Point(Math.Abs(X), Math.Abs(Y)); 
}