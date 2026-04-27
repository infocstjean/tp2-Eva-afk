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

        return "[" + string.Join(", ", jsonItems) + "]";
    }
}