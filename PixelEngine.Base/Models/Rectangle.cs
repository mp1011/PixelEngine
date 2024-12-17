public record Rectangle(int X, int Y, int Width, int Height)
{
    public bool Contains(Point p) =>
        p.X >= X &&
        p.Y >= Y &&
        p.X <= X + Width &&
        p.Y <= Y + Height;
}