namespace shared;

public record HttpRequestParser
{
    public static HttpRequest ParseRequest(string raw)
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
}