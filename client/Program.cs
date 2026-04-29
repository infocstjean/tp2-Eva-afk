namespace client;

using System.Net.Sockets;
using System.Reflection;
using System.Text;
using shared;

public class Client
{
    private static readonly CancellationTokenSource cts = new CancellationTokenSource();
    public static async Task Main()
    {
        while (!cts.IsCancellationRequested)
        {
            try
            {
                await HandleClient(cts.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Requête de fermeture. Fermeture du client...");
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
    public static async Task HandleClient(CancellationToken ct)
    {
        Console.WriteLine("--- Nouvelle requête ---");
        Console.Write("Entrez l'URL ou exit (ex: http://localhost:8088/api/messages) : ");
        string? url = Console.ReadLine();
        if (url == "exit" || url == "quit")
        {
            cts.Cancel();
            throw new OperationCanceledException();
        }
        Console.Write("Méthode (GET, POST, PUT, DELETE, PATCH) : ");
        string? method = Console.ReadLine()?.ToUpper();

        string body = "";
        if (method =="PUT" || method =="POST" || method == "PATCH")
        {
            Console.Write("Entrez votre message : ");
            body = Console.ReadLine() ?? "";
        }

        if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(method)) return;

        string? cutUrl = url.Replace("http://", "");
        int slashIndex = cutUrl.IndexOf("/");
        string addressAndPort = cutUrl.Substring(0, slashIndex);
        string path = cutUrl.Substring(slashIndex);

        string host = addressAndPort.Trim();
        int port = 8088;

        if (addressAndPort.Contains(":"))
        {
            string[] parts = addressAndPort.Split(':');
            host = parts[0].Trim();
            port = Convert.ToInt32(parts[1].Trim());
        }

        using TcpClient client = new TcpClient();
        await client.ConnectAsync(host, port);
        using NetworkStream stream = client.GetStream();

        await SendRequest(url, method, path, host, stream, ct, Encoding.UTF8.GetBytes(body));
        await ReceiveResponse(stream, ct);
    }
    public static async Task SendRequest(string url, string method, string path, string host, NetworkStream stream, CancellationToken ct, byte[] body)
    {
        HttpRequest request = new HttpRequest
        {
            Method = method,
            Path = path,
            Body = body
        };

        byte[] byteRequest = HttpRequestWriter.ToBytes(request, host);
        await stream.WriteAsync(byteRequest);
    }
    public static async Task ReceiveResponse(NetworkStream stream, CancellationToken ct)
    {
        // taille de header en http
        byte[] bytes = new byte[8192];
        int bytesRead = await stream.ReadAsync(bytes, 0, bytes.Length);
        if (bytesRead == 0) return;
        string raw = Encoding.UTF8.GetString(bytes);
        Console.WriteLine("--- Réponse du serveur ---");
        Console.WriteLine(raw);
    }
}