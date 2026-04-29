using System.Text;

namespace shared;

public record HttpResponseParser
{
    public static HttpResponse ParseResponse(string raw)
    {
        int separator = raw.IndexOf("\r\n\r\n");
        string headers = raw.Substring(0, separator);
        string body = raw.Substring(separator + 4);

        string[] lines = headers.Split("\r\n");
        string[] parts = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 3)
        {
            throw new Exception("Ligne de requete invalide");
        }

        HttpResponse response = new HttpResponse
        {
            Version = parts[0],
            StatusCode = Convert.ToInt32(parts[1]),
            ReasonPhrase = parts[2]
        };
        for (int i = 0; i < lines.Length - 1; i++)
        {
            string line = lines[i];
            if (line == "") continue;
            int searchedIndex = line.IndexOf(':');
            if(i > 0)
            {
                string key = line.Substring(0, searchedIndex);
                string value = line.Substring(searchedIndex + 1);
                response.Headers[key] = value;
            }
        }
        response.Body = Encoding.UTF8.GetBytes(body);
        return response;
    }
}