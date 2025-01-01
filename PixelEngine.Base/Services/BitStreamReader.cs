public class BitStreamReader
{
    private readonly byte[] _data;
    private int _byteIndex;
    private int _bitIndex;

    public BitStreamReader(byte[] data)
    {
        _data = data;
    }

    public void Seek(int bytes)
    {
        _byteIndex = bytes;
        _bitIndex = 0;
    }

    public byte ReadNextBits(int bits)
    {
        if(_bitIndex + bits > 8)
        {
            int bitsOverflow = (_bitIndex + bits) - 8;

            int lowBits = bits - bitsOverflow;

            var low = ReadNextBits(lowBits);
            var high = ReadNextBits(bitsOverflow);

            return (byte)((high << lowBits) + low);
        }

        var mask = Mask(bits) << _bitIndex;
        var value = (_data[_byteIndex] & mask) >> _bitIndex;

        _bitIndex += bits;
        if (_bitIndex == 8)
        {
            _bitIndex = 0;
            _byteIndex++;
        }

        return (byte)value;
    }

    private byte Mask(int bits) =>
        bits switch
        {
            1 => 1,
            2 => 3,
            3 => 7,
            4 => 15,
            5 => 31,
            6 => 63,
            7 => 127,
            8 => 255
        };
}

