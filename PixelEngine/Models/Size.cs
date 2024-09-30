namespace PixelEngine.Models;

public record Size(int Width, int Height)
{
    public static Size operator *(Size s, int factor)
    {
        return new Size(s.Width * factor, s.Height * factor);
    }
}
