using System.Text;

namespace shared;

public class HttpRequestWriter
{
    public static byte[] ToBytes(HttpRequest request, string host)
    {
        var sb = new StringBuilder();
        sb.Append($"{request.Method} {request.Path} {request.Version}\r\n");
        sb.Append($"Host: {host}\r\n");
        if (request.Body != null && request.Body.Length > 0)
        {
            if (!request.Headers.ContainsKey("Content-Length"))
            {
                sb.Append($"Content-Length: {request.Body.Length}\r\n");
            }
        }

        foreach (var pair in request.Headers)
        {
            sb.Append($"{pair.Key}: {pair.Value}\r\n");
        }

        sb.Append("\r\n");
        byte[] headers = Encoding.UTF8.GetBytes(sb.ToString());
        return request.Body != null
            ? headers.Concat(request.Body).ToArray()
            : headers;
    }
}