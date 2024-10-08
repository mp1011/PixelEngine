﻿public class WrappingPoint
{
    private readonly Point _limit;
    private int _x, _y;
    public int X
    {
        get => _x;
        set => _x = value.NMod(_limit.X);
    }

    public int Y
    {
        get => _y;
        set => _y = value.NMod(_limit.Y);
    }

    public WrappingPoint(int limitX, int limitY)
    {
        _limit = new Point(limitX, limitY);
    }

    public Point ToPoint() => new Point(X, Y);

    public override string ToString() => $"{X},{Y}";
    public WrappingPoint(Point limit) : this (limit.X, limit.Y) { }
    public WrappingPoint(Size limit) : this(limit.Width, limit.Height) { }

    public static implicit operator Point(WrappingPoint point) => point.ToPoint();
}
