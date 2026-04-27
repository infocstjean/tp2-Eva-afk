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

    public static HttpResponse MethodNotAllowed()
    {
        byte[] body = Encoding.UTF8.GetBytes("Method Not Allowed");
        var response = new HttpResponse
        {
            StatusCode = 405,
            ReasonPhrase = "Method Not Allowed",
            Body = body
        };
        response.Headers["Content-Type"] = "text/plain; charset=utf-8";
        response.Headers["Content-Length"] = body.Length.ToString();
        response.Headers["Connection"] = "close";
        return response;
    }

    public static HttpResponse Html(string htmlText)
    {
        byte[] body = Encoding.UTF8.GetBytes(htmlText);
        var response = new HttpResponse
        {
            StatusCode = 200,
            ReasonPhrase = "OK",
            Body = body
        };
        response.Headers["Content-Type"] = "text/html; charset=utf-8";
        response.Headers["Content-Length"] = body.Length.ToString();
        response.Headers["Connection"] = "close";
        return response;
    }

    public static HttpResponse Message(int messageId, string message)
    {
        string text = Utils.MessageToJson(messageId, message);
        byte[] body = Encoding.UTF8.GetBytes(text);
        var response = new HttpResponse
        {
            StatusCode = 200,
            ReasonPhrase = "OK",
            Body = body
        };
        response.Headers["Content-Type"] = "application/json; charset=utf-8";
        response.Headers["Content-Length"] = body.Length.ToString();
        response.Headers["Connection"] = "close";
        return response;
    }

    public static HttpResponse MessageList(Dictionary<int, string> messages)
    {
        string jsonContent = Utils.MessageListToJson(messages);

        byte[] body = Encoding.UTF8.GetBytes(jsonContent);

        var response = new HttpResponse
        {
            StatusCode = 200,
            ReasonPhrase = "OK",
            Body = body
        };

        response.Headers["Content-Type"] = "application/json; charset=utf-8";
        response.Headers["Content-Length"] = body.Length.ToString();
        response.Headers["Connection"] = "close";

        return response;
    }

    public static HttpResponse NotFound()
    {
        byte[] body = Encoding.UTF8.GetBytes("Not Found");
        var response = new HttpResponse
        {
            StatusCode = 404,
            ReasonPhrase = "Not Found",
            Body = body
        };
        response.Headers["Content-Type"] = "text/plain; charset=utf-8";
        response.Headers["Content-Length"] = body.Length.ToString();
        response.Headers["Connection"] = "close";
        return response;
    }
}