public class DiskResourceLoader
{
    public static byte[] Load(string path) => File.ReadAllBytes($"{ContentFolder}/{path}");

    public static byte[][] LoadAll(string folder) =>
        new DirectoryInfo($"{ContentFolder}/{folder}")
            .GetFiles("*.*")
            .Select(p=> File.ReadAllBytes(p.FullName))
            .ToArray();

    private static DirectoryInfo? ContentFolder
    {
        get
        {
            //replace with embedded resources
            return new DirectoryInfo(@"D:\GitHub\PixelEngine\PixelEngine.SampleGame\Content");
        }
    }
}
