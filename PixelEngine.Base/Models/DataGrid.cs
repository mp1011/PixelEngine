public abstract class DataGrid<T>
{
    public int Width { get; }
    public int Height { get; }

    public Size Size => new Size(Width, Height);

    public int Length => Width * Height;

    public abstract T this[int index] { get; set; }

    public T this[int x, int y]
    {
        get => this[(y * Width) + x];
        set => this[(y * Width) + x] = value;
    }

    public T this[Point p] => this[p.X, p.Y];

    public DataGrid(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public void Rotate(int rX, int rY)
    {
        var copy = new ArrayDataGrid<T>(Width, Height);

        copy.ForEach((x, y) =>
        {
            copy[x, y] = this[x, y];
        });

        ForEach((x, y) =>
        {
            var destX = (x + rX) % Width;
            var destY = (y + rY) % Height;

            this[destX, destY] = copy[x, y];
        });
    }

    public void ForEach(Action<int,int> action)
    {
        for(int y = 0; y < Height; y++)
        {
            for(int x = 0; x < Width; x++)
            {
                action(x, y);
            }
        }
    }

    public void ForEach(Point from, Point to, Action<int, int> action)
    {
        for (int y = from.Y; y <= to.Y; y++)
        {
            for (int x = from.X; x <= to.X; x++)
            {
                if(x >= 0 && y >= 0 && x < Width && y < Height)
                    action(x, y);
            }
        }
    }

    public void ForEachInRow(int row, Action<int,int> action)
    {
        for (int x = 0; x < Width; x++)
        {
            action(x, row);
        }
    }

    public void MapFrom<TFrom>(DataGrid<TFrom> source, Func<int,int,TFrom,T> map)
    {
        ForEach((x, y) =>
        {
            this[x, y] = map(x, y, source[x, y]);
        });
    }

    public void CopyFrom(DataGrid<T> source, Rectangle sourceRegion, Point destination)
    {
        source.ForEach((x, y) =>
        {
            var srcPt = new Point(x, y);
            if (sourceRegion.Contains(srcPt))
            {
                this[destination.X + (x - sourceRegion.X),
                    destination.Y + (y - sourceRegion.Y)] = source[x, y];
            }
        });
    }


}

public class ArrayDataGrid<T> : DataGrid<T>
{
    private T[] _data;
    public override T this[int index]
    {
        get => _data[index];
        set => _data[index] = value;
    }

    public ArrayDataGrid(int width, int height)
        : base(width, height)
    {
        _data = new T[width * height];
    }

    public T[] ToArray() => _data;
}