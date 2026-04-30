namespace client;

using System.Net.Sockets;
using System.Text;
using shared;

public class Client
{
    private static readonly CancellationTokenSource Cts = new();

    public static async Task Main()
    {
        while (!Cts.IsCancellationRequested)
        {
            try
            {
                await HandleClient(Cts.Token);
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
            Cts.Cancel();
            throw new OperationCanceledException();
        }

        Console.Write("Méthode (GET, POST, PUT, DELETE, PATCH) : ");
        string? method = Console.ReadLine()?.ToUpper();

        string body = "";
        if (method == "PUT" || method == "POST" || method == "PATCH")
        {
            Console.Write("Entrez votre message : ");
            body = Console.ReadLine() ?? "";
        }

        if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(method))
        {
            Console.WriteLine("URL ou méthode vide");
            return;
        }

        try
        {
            string cutUrl = url.Replace("http://", "");
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
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Erreur dans le format de l'url");
        }
    }

    public static async Task SendRequest(string url, string method, string path, string host, NetworkStream stream,
        CancellationToken ct, byte[] body)
    {
        HttpRequest request = new HttpRequest
        {
            Method = method,
            Path = path,
            Body = body
        };
        request.Headers.Add("Accept", "*/*");
        byte[] byteRequest = HttpRequestWriter.ToBytes(request, host);
        await stream.WriteAsync(byteRequest);
    }

    public static async Task ReceiveResponse(NetworkStream stream, CancellationToken ct)
    {
        string raw = await Utils.ReadHttpHeadersAsync(stream);
        Console.WriteLine("--- Réponse du serveur ---");
        Console.WriteLine(raw);
    }
}