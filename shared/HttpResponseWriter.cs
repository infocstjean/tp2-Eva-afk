using System.Text;
namespace shared;

public class HttpResponseWriter
{
    // 1. Added 'public static' so the Server can call this directly
    public static HttpResponse Handle(HttpRequest request)
    {
        if (request.Method != "GET")
            return Responses.MethodNotAllowed();
        if (request.Path == "/")
            return Responses.Html(File.ReadAllText("index.html"));
        if (request.Path == "/bonjour")
            return Responses.Text("Bonjour!");
        return Responses.NotFound();
    }

    // 2. Added 'public' (already had static)
    public static byte[] ToBytes(HttpResponse response)
    {
        var sb = new StringBuilder();
        sb.Append($"{response.Version} {response.StatusCode} ");
        sb.Append($"{response.ReasonPhrase}\r\n");
        foreach (var pair in response.Headers)
            sb.Append($"{pair.Key}: {pair.Value}\r\n");
        sb.Append("\r\n");
        
        byte[] headers = Encoding.UTF8.GetBytes(sb.ToString());
        return headers.Concat(response.Body).ToArray();
    }
}
