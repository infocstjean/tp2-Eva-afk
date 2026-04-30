using System.Runtime.CompilerServices;
using System.Text;

namespace shared;

public class HttpResponseWriter
{
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

    public static HttpResponse CreateResponse(int code, string phrase, byte[] body, string contentType)
    {
        var response = new HttpResponse
        {
            StatusCode = code,
            ReasonPhrase = phrase,
            Body = body
        };
        response.Headers["Content-Type"] = contentType;
        response.Headers["Content-Length"] = body.Length.ToString();
        response.Headers["Connection"] = "close";
        return response;
    }
    public static HttpResponse MethodNotAllowed()
    {

        return CreateResponse(405, "Method Not Allowed", Array.Empty<byte>(), "text/plain");
    }
    public static HttpResponse NotFound()
    {
        return CreateResponse(404, "Not Found", Array.Empty<byte>(), "text/plain");
    }
    public static HttpResponse BadRequest()
    {
        return CreateResponse(400, "Bad Request", Array.Empty<byte>(), "text/plain");

    }
    public static HttpResponse InternalError()
    {
        return CreateResponse(500, "Internal Server Error", Array.Empty<byte>(), "text/plain");

    }
    public static HttpResponse Post(int id, string message)
    {
        return CreateResponse(201, "Created", Encoding.UTF8.GetBytes($"Nouveau message: {Utils.MessageToJson(id, message)}"), "text/plain");
    }
    public static HttpResponse Put(int id)
    {
        return CreateResponse(200, "OK", Array.Empty<byte>(), "text/plain");
    }
    public static HttpResponse Delete(int id)
    {

        return CreateResponse(204, "No Content", Array.Empty<byte>(), "text/plain");
    }
    public static HttpResponse Patch(int id)
    {
        return CreateResponse(200, "OK", Array.Empty<byte>(), "text/plain");
    }
    public static HttpResponse GetListMessages(Dictionary<int, string> messages)
    {
        string messagesJson = Utils.MessageListToJson(messages);
        return CreateResponse(200, "OK", Encoding.UTF8.GetBytes(messagesJson), "application/json");
    }
    public static HttpResponse GetSingleMessage(int id, string message)
    {
        string messageJson = Utils.MessageToJson(id, message);
        return CreateResponse(200, "OK", Encoding.UTF8.GetBytes(messageJson), "application/json");
    }
    public static HttpResponse GetHtml(string htmlContent)
    {
        return CreateResponse(200, "OK", Encoding.UTF8.GetBytes(htmlContent), "text/html");
    }
}