public record Rectangle(int X, int Y, int Width, int Height)
{
    public int Top => Y;
    public int Left => X;
    public int Bottom => Y + Height;
    public int Right => X + Width;

    public bool Intersects(Rectangle other)
    {
        return !(
            other.Right < Left
            || other.Left > Right
            || other.Bottom < Top
            || other.Top > Bottom);
    }

    public bool Contains(Point p) =>
        p.X >= X &&
        p.Y >= Y &&
        p.X <= X + Width &&
        p.Y <= Y + Height;
}