public static class Debug
{
    public static string Text1 { get; set; }
    public static string Text2 { get; set; }

    public static  CoordinateTranslator CoordinateTranslator { get; set; }

    public static List<ColliderWatch> WatchedColliders { get; } = new List<ColliderWatch>();

}

