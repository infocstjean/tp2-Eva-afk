using System.Net.Sockets;
using System.Text;

namespace shared;

public class Utils
{
    public static string MessageToJson(int messageId, string message)
    {
        return "{\"id\": " + messageId + ", \"text\": \"" + message + "\"}";
    }

    public static string MessageListToJson(Dictionary<int, string> messages)
    {
        List<string> jsonItems = new List<string>();

        foreach (var message in messages)
        {
            jsonItems.Add(MessageToJson(message.Key, message.Value));
        }

        return string.Join(", ", jsonItems);
    }

    public static async Task<string> ReadHttpHeadersAsync(NetworkStream stream, CancellationToken  ct)
    {
        byte[] buffer = new byte[1024];
        using var ms = new MemoryStream();
        while (true)
        {
            int n = await stream.ReadAsync(buffer, 0, buffer.Length, ct);
            if (n == 0)
                break;
            ms.Write(buffer, 0, n);
            string text = Encoding.UTF8.GetString(ms.ToArray());
            if (text.Contains("\r\n\r\n"))
                return text;
        }

        throw new Exception("Requete HTTP incomplete");
    }
}