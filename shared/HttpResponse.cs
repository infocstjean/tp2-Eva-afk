namespace shared;

public class HttpResponse
{
    public string Version { get; set; } = "HTTP/1.1";
    public int StatusCode { get; set; }
    public string ReasonPhrase { get; set; } = "";
    public Dictionary<string, string> Headers { get; } = new();
    public byte[] Body { get; set; } = Array.Empty<byte>();
    
}