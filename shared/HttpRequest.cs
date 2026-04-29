namespace shared;

public class HttpRequest
{
    public string Version { get; set; } = "HTTP/1.1";
    public string Method { get; set; } = "GET";
    public string Path { get; set; } = "/";
    public Dictionary<string, string> Headers { get; set; } = new();
    public byte[]? Body { get; set; } = null;
}