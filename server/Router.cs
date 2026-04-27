namespace server;
using shared;
public class Router
{
    public static HttpResponse Handle(HttpRequest request, Dictionary<int, string> messages)
    {
        if (request.Method == "GET")
        {
            if (request.Path == "/")
            {
                return HttpResponseWriter.Html(HtmlFileProvider.SendHtmlPage());
            }
            else if (request.Path == "/api/messages")
            {
                return HttpResponseWriter.MessageList(messages);
            }
            else if (request.Path.Contains("/api/messages/"))
            {
                int id = GetIdFromRequest(request);
                return HttpResponseWriter.Message(id, messages[id]);
            }
        }
        else if (request.Method == "POST")
        {
            if (request.Path.Contains("/api/messages/"))
            {
            }
        }
        else if (request.Method == "PUT")
        {
            if (request.Path.Contains("/api/messages/"))
            {
                int id = GetIdFromRequest(request);
            }
        }
        else if (request.Method == "DELETE")
        {
            if (request.Path.Contains("/api/messages/"))
            {
                int id = GetIdFromRequest(request);
                messages.Remove(id);
            }
        }
        else if (request.Method == "PATCH")
        {
            if (request.Path.Contains("/api/messages/"))
            {
                int id = GetIdFromRequest(request);
            }
        }
        return HttpResponseWriter.MethodNotAllowed();
    }

    public static int GetIdFromRequest(HttpRequest request)
    {
        return Convert.ToInt32(request.Path.Split('/').Last());
    }
}