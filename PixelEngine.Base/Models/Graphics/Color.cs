public record struct Color(byte R, byte G, byte B, byte A)
{
    public const int Bytes = 4;
    public Color(byte R, byte G, byte B) : this(R, G, B, 255) { }

    public Color(int R, int G, int B) : this((byte)R, (byte)G, (byte)B, 255) { }
}