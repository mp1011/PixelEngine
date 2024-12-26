public static class ByteExtensions
{
    public static byte[] EndianSwap(this byte[] data)
    {
        for (int i = 0; i < data.Length; i += 2)
        {
            var b0 = data[i];
            data[i] = data[i + 1];
            data[i + 1] = b0;
        }
        return data;
    }
}

