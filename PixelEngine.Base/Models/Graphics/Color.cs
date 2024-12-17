public record Color(byte R, byte G, byte B, byte A)
{
    public const int Bytes = 4;
    public Color(byte R, byte G, byte B) : this(R, G, B, 255) { }
}