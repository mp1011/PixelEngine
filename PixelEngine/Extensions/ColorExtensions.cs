static class ColorExtensions
{
    public static XnaColor ToXnaColor(this Color color) => new XnaColor(color.R, color.G, color.B);
}