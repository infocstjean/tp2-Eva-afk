namespace shared;

public static class HtmlFileProvider
{
    public static string SendHtmlPage()
    {
        return File.ReadAllText("/server/page.html");
    }
}