namespace shared;

public static class HtmlFileProvider
{
    private static readonly string Path = "page.html";

    public static string SendHtmlPage()
    {
        if (!File.Exists(Path))
        {
            return "<html><body>Fichier non trouvé</body></html>";
        }

        return File.ReadAllText(Path);
    }
}