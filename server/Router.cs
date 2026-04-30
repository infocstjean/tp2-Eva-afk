namespace server;

using System.Runtime.InteropServices;
using System.Text;
using shared;

public class Router
{
    public static int nextId = 0;

    public static HttpResponse Handle(HttpRequest request, Dictionary<int, string> messages)
    {
        try
        {
            
        if (request.Method == "GET")
        {
            if (request.Path == "/")
            {
                return HttpResponseWriter.GetHtml(HtmlFileProvider.SendHtmlPage());
            }
            else if (request.Path == "/api/messages")
            {
                return HttpResponseWriter.GetListMessages(messages);
            }
            else if (request.Path.Contains("/api/messages/"))
            {
                int id = GetIdFromRequest(request);
                if(!VerifyIdContainedInMessages(id, messages)) return HttpResponseWriter.NotFound();
                return HttpResponseWriter.GetSingleMessage(id, messages[id]);
            }
        }
        else if (request.Method == "POST")
        {
            if (request.Path.Contains("/api/messages"))
            {
                string body = Encoding.UTF8.GetString(request.Body ?? Array.Empty<byte>());
                nextId++;
                messages.Add(nextId, body);
                return HttpResponseWriter.Post(nextId, messages[nextId]);
            }
        }
        else if (request.Method == "PUT")
        {
            if (request.Path.Contains("/api/messages/"))
            {
                int id = GetIdFromRequest(request);
                if(!VerifyIdContainedInMessages(id, messages)) return HttpResponseWriter.NotFound();

                return HttpResponseWriter.Put(id);
            }
        }
        else if (request.Method == "DELETE")
        {
            if (request.Path.Contains("/api/messages/"))
            {
                int id = GetIdFromRequest(request);
                if(!VerifyIdContainedInMessages(id, messages)) return HttpResponseWriter.NotFound();
                messages.Remove(id);
                return HttpResponseWriter.Delete(id);
            }
        }
        else if (request.Method == "PATCH")
        {
            if (request.Path.Contains("/api/messages/"))
            {
                int id = GetIdFromRequest(request);
                if(!VerifyIdContainedInMessages(id, messages)) return HttpResponseWriter.NotFound();

                return HttpResponseWriter.Patch(id);
            }
        }

        return HttpResponseWriter.BadRequest();
        }
        catch (Exception)
        {
           return HttpResponseWriter.InternalError(); 
        }
    }

    public static int GetIdFromRequest(HttpRequest request)
    {
        return Convert.ToInt32(request.Path.Split('/').Last());
    }

    public static bool VerifyIdContainedInMessages(int id, Dictionary<int, string> messages)
    {
        if (messages.ContainsKey(id))
        {
            return true;
        }

        return false;
    }
}