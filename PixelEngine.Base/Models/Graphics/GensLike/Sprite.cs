public class Sprite
{
    private byte _vpLow, _vpHigh, _next, _size, _gfxLow, _gfxHighAndFlags, _hpLow, _hpHigh;


    public Sprite()
    {

    }

    public Sprite(byte[] data, int index)
    {
        _vpLow = data[index];
        _vpHigh = data[index + 1];
        _next = data[index + 2];
        _size = data[index + 3];
        _gfxLow = data[index + 4];
        _gfxHighAndFlags = data[index + 5];
        _hpLow = data[index + 6];
        _hpHigh = data[index + 7];
    }

    public int VerticalPos
    {
        get => _vpLow + (_vpHigh << 8);
        set
        {
            _vpLow = (byte)value;
            _vpHigh = (byte)(value >> 8);
        }
    }

    public int HorizontalPos
    {
        get => _hpLow + (_hpHigh << 8);
        set
        {
            _hpLow = (byte)value;
            _hpHigh = (byte)(value >> 8);
        }
    }


    public byte Next
    {
        get => _next;
        set => _next = value;
    }

    public byte VSize
    {
        get => (byte)(_size & 3);
        set
        {
            _size = (byte)((_size & 252) | value);
        }
    }

    public byte HSize
    {
        get => (byte)((_size & 12) >> 2);
        set
        {
            _size = (byte)((_size & 245) | (value<<2));
        }
    }

    public int Tile
    {
        get => _gfxLow + ((_gfxHighAndFlags & 7) << 8);
        set
        {
            _gfxLow = (byte)value;
            _gfxHighAndFlags = (byte)((_gfxHighAndFlags & 248) + (value >> 8));
        }
    }

    public bool HorizontalFlip
    {
        get => (_gfxHighAndFlags & 8) != 0;
        set
        {
            if (value)
                _gfxHighAndFlags = (byte)(_gfxHighAndFlags | 8);
            else
                _gfxHighAndFlags = (byte)(_gfxHighAndFlags & 247);
        }
    }

    public bool VerticalFlip
    {
        get => (_gfxHighAndFlags & 16) != 0;
        set
        {
            if (value)
                _gfxHighAndFlags = (byte)(_gfxHighAndFlags | 16);
            else
                _gfxHighAndFlags = (byte)(_gfxHighAndFlags & 239);
        }
    }

    public bool Priority
    {
        get => (_gfxHighAndFlags & 128) != 0;
        set
        {
            if (value)
                _gfxHighAndFlags = (byte)(_gfxHighAndFlags | 128);
            else
                _gfxHighAndFlags = (byte)(_gfxHighAndFlags & 127);
        }
    }

    public byte PaletteIndex
    {
        get => (byte)((_gfxHighAndFlags & 96) >> 5);
        set
        {
            _gfxHighAndFlags = (byte)((_gfxHighAndFlags & 159) | (value << 5));
        }
    }
}