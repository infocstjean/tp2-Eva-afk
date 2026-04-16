namespace shared;

public class HttpRequestParser
{
    static HttpRequest ParseRequest(string raw)
    {
        string[] lines = raw.Split("\r\n");
        string[] parts = lines[0].Split(' ',
            StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3)
        {
            throw new Exception("Ligne de requete invalide");
        }

        return new HttpRequest
        {
            Method = parts[0],
            Path = parts[1],
            Version = parts[2]
        };
    }

    static Dictionary<string, string> ParseHeaders(string raw)
    {
        var headers = new Dictionary<string, string>();
        string[] lines = raw.Split("\r\n");
        for (int i = 1; i < lines.Length; i++)
        {
            if (lines[i] == "") break;
            int idx = lines[i].IndexOf(':');
            if (idx <= 0) continue;
            string name = lines[i][..idx].Trim();
            string value = lines[i][(idx + 1)..].Trim();
            headers[name] = value;
        }

        return headers;
    }
}