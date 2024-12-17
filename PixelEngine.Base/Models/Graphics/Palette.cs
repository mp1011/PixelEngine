
public unsafe class Palette : IDisposable
{
    private Color[] _colors;
    private GCHandle _handle;
    private nint _pointer;
    private bool disposedValue;

    public int Length => _colors.Length;

    public Palette()
    {
        _colors = Enumerable.Range(0, 64)
            .Select(p => new Color((byte)(p * 32), (byte)p, 0))
            .ToArray();

        _handle = GCHandle.Alloc(_colors[0], GCHandleType.Pinned);
        _pointer = _handle.AddrOfPinnedObject();
    }

    public nint ColorPointer(int index) => _pointer + (index * Color.Bytes);

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            _handle.Free();
            _pointer = 0;
            disposedValue = true;
        }
    }
    ~Palette()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

