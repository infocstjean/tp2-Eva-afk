using System.Text;

namespace shared;

public static class HttpResponseWriter
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
        return CreateResponse(405, "Method Not Allowed", Encoding.UTF8.GetBytes("Méthode non permise"), "text/plain");
    }

    public static HttpResponse NotFound()
    {
        return CreateResponse(404, "Not Found",
            Encoding.UTF8.GetBytes("Message n'existe pas ou ne peut pas être trouvé"), "text/plain");
    }

    public static HttpResponse BadRequest()
    {
        return CreateResponse(400, "Bad Request", Encoding.UTF8.GetBytes("Requête mal formulée"), "text/plain");
    }

    public static HttpResponse InternalError()
    {
        return CreateResponse(500, "Internal Server Error",
            Encoding.UTF8.GetBytes("Erreur dans le fonctionnement interne du serveur"), "text/plain");
    }

    public static HttpResponse Post(int id, string message)
    {
        return CreateResponse(201, "Created",
            Encoding.UTF8.GetBytes($"Nouveau message: {Utils.MessageToJson(id, message)}"), "text/plain");
    }

    public static HttpResponse PutOrPatch(int id, string message)
    {
        return CreateResponse(200, "OK", Encoding.UTF8.GetBytes($"Message modifié: {Utils.MessageToJson(id, message)}"),
            "text/plain");
    }

    public static HttpResponse Delete(int id)
    {
        return CreateResponse(204, "No Content", Encoding.UTF8.GetBytes("Suppression réussie"), "text/plain");
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