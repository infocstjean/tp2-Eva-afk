namespace shared;

public class HttpRequest
{
    public string Version { get; set; } = "HTTP/1.1";
    public string Method { get; set; } = "GET";
    public string Path { get; set; } = "/";
}