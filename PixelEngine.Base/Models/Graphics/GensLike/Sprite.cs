public class Sprite
{
    public const int Size = 8;

    private ArraySegment<byte> _data;

    private byte _vpLow
    {
        get => _data[0];
        set => _data[0] = value;
    }

    public byte _vpHigh
    {
        get => _data[1];
        set => _data[1] = value;
    }

    public byte _next
    {
        get => _data[2];
        set => _data[2] = value;
    }

    public byte _size
    {
        get => _data[3];
        set => _data[3] = value;
    }

    public byte _gfxLow
    {
        get => _data[4];
        set => _data[4] = value;
    }

    public byte _gfxHighAndFlags
    {
        get => _data[5];
        set => _data[5] = value;
    }

    private byte _hpLow
    {
        get => _data[6];
        set => _data[6] = value;
    }

    public byte _hpHigh
    {
        get => _data[7];
        set => _data[7] = value;
    }


    public Sprite()
    {
        _data = new ArraySegment<byte>(new byte[Sprite.Size]);
    }

    public void UpdateMemory(byte[] vram, int address)
    {
        _data = new ArraySegment<byte>(vram, address, Sprite.Size);
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