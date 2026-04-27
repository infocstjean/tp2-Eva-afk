using System.Text;

namespace shared;

public class HttpRequestWriter
{
    public static async Task<string> WriteRequest(HttpRequest req)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"{req.Method} {req.Path} {req.Version}\r\n");
        sb.Append($"Host: {host}\r\n");
        sb.Append("Connection: close\r\n");
        sb.Append("\r\n");
        return sb.ToString();
    }
}