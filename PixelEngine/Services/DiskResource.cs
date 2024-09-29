using System.Reflection;

namespace PixelEngine.Services;

public class DiskResourceLoader
{
    public byte[] Load(string path) => File.ReadAllBytes($"{ContentFolder}/{path}"); 
    private DirectoryInfo ContentFolder
    {
        get
        {
            var file = new FileInfo(Assembly.GetExecutingAssembly().Location);
            return file.Directory.GetAncestor("PixelEngine").GetChild("Content");
        }
    }
}
