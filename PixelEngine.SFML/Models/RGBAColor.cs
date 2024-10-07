internal record RGBAColor(byte[] Buffer, int BufferOffset)
{    
    public byte R
    {
        get => Buffer[BufferOffset];
        set => Buffer[BufferOffset] = value;
    }

    public byte G
    {
        get => Buffer[BufferOffset + 1];
        set => Buffer[BufferOffset + 1] = value;
    }

    public byte B
    {
        get => Buffer[BufferOffset + 2];
        set => Buffer[BufferOffset + 2] = value;
    }

    public void Init()
    {
        R = 0; G = 0; B = 0;
        Buffer[BufferOffset + 3] = 255;
    }
}

