using System.Text;

namespace shared;

public class Responses
{
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

    public static HttpResponse Text(string text)
    {
        byte[] body = Encoding.UTF8.GetBytes(text);
        var response = new HttpResponse
        {
            StatusCode = 200,
            ReasonPhrase = "OK",
            Body = body
        };
        response.Headers["Content-Type"] = "text/plain; charset=utf-8";
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