namespace shared;

public class Router
{
    HttpResponse Handle(HttpRequest request)
    {
        if (request.Method == "GET")
        {
            if (request.Path == "/")
            {
                return Responses.Html(File.ReadAllText("index.html"));
            }
            else if (request.Path == "/api/messages")
            {
            }
            else if (request.Path == "/api/messages/")
            {
            }
        }
        else if (request.Method == "POST")
        {
            if (request.Path == "/api/messages")
            {
            }
        }
        else if (request.Method == "PUT")
        {
            if (request.Path.Contains("/api/messages/"))
            {
            }
        }
        else if (request.Method == "DELETE")
        {
            if (request.Path == "/api/messages/")
            {
            }
        }
        else if (request.Method == "PATCH")
        {
            if ()
            {
            }
        }
        else
        {
            return Responses.MethodNotAllowed();
        }
    }
}