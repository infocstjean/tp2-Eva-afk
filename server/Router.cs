namespace server;

using System.Text;
using shared;

public class Router
{
    private static readonly object Gate = new();
    public static int NextId;

    public static HttpResponse Handle(HttpRequest request, Dictionary<int, string> messages)
    {
        lock (Gate)
        {
            try
            {
                if (request.Path == "/" && request.Method != "GET")
                {
                    return HttpResponseWriter.MethodNotAllowed();
                }

                if (request.Path == "/api/messages" && request.Method != "GET" && request.Method != "POST")
                {
                    return HttpResponseWriter.MethodNotAllowed();
                }

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
                        if (!VerifyIdContainedInMessages(id, messages)) return HttpResponseWriter.NotFound();
                        return HttpResponseWriter.GetSingleMessage(id, messages[id]);
                    }
                }
                else if (request.Method == "POST")
                {
                    if (request.Path.Equals("/api/messages"))
                    {
                        string body = Encoding.UTF8.GetString(request.Body ?? Array.Empty<byte>());
                        NextId++;
                        messages.Add(NextId, body);
                        return HttpResponseWriter.Post(NextId, messages[NextId]);
                    }
                }
                else if (request.Method == "PUT" || request.Method == "PATCH")
                {
                    if (request.Path.Contains("/api/messages/"))
                    {
                        int id = GetIdFromRequest(request);
                        if (!VerifyIdContainedInMessages(id, messages)) return HttpResponseWriter.NotFound();
                        string bodyChange = Encoding.UTF8.GetString(request.Body ?? Array.Empty<byte>());
                        if (bodyChange != "")
                        {
                            messages[id] = bodyChange;
                            return HttpResponseWriter.PutOrPatch(id, messages[id]);
                        }
                    }
                }
                else if (request.Method == "DELETE")
                {
                    if (request.Path.Contains("/api/messages/"))
                    {
                        int id = GetIdFromRequest(request);
                        if (!VerifyIdContainedInMessages(id, messages)) return HttpResponseWriter.NotFound();
                        messages.Remove(id);
                        return HttpResponseWriter.Delete(id);
                    }
                }

                return HttpResponseWriter.BadRequest();
            }
            catch (Exception)
            {
                return HttpResponseWriter.InternalError();
            }
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