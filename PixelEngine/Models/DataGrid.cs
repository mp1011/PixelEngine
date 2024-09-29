using System.Text;

namespace PixelEngine.Models;

public abstract class DataGrid<T>
{
    public int Width { get; }
    public int Height { get; }

    public Point Size => new Point(Width, Height);

    public abstract T this[int index] { get; set; }

    public T this[int x, int y]
    {
        get => this[(y * Width) + x];
        set => this[(y * Width) + x] = value;
    }

    public DataGrid(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public void ForEach(Action<int,int> action)
    {
        for(int y = 0; y < Height; y++)
        {
            for(int  x = 0; x < Width; x++)
            {
                action(x, y);
            }
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

public class BitArrayDataGrid : DataGrid<byte>
{
    private NBitArray _data;
    public override byte this[int index]
    {
        get => _data[index];
        set => _data[index] = value;
    }

    public BitArrayDataGrid(int width, int height, NBitArray data)
        : base(width, height)
    {
        _data = data;
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        int curY = 0;

        ForEach((x, y) =>
        {
            if (y > curY)
            {
                curY = y;
                sb.AppendLine();
            }

            var value = this[x, y].ToString("X1");
            sb.Append(value);
            
        });

        return sb.ToString();
    }
}
