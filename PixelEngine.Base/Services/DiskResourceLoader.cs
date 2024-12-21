public class DiskResourceLoader
{
    public static byte[] Load(string path) => File.ReadAllBytes($"{ContentFolder}/{path}"); 
    private static DirectoryInfo? ContentFolder
    {
        get
        {
            var directory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
            int maxDepth = 32;
            while (maxDepth-- > 0 && directory != null)
            {
                var contentFolder = directory.GetChild("Content");
                if (contentFolder != null)
                    return contentFolder;

                directory = directory.Parent;
            }

            throw new Exception("Unable to find content folder");           
        }
    }
}
