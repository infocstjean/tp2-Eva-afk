namespace client;

using System.Net.Sockets;
using System.Text;
using shared;

public class Client
{
    public static async Task Main()
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        while (!cts.IsCancellationRequested)
        {
            try
            {
                await HandleClient(cts);
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

    public static async Task HandleClient(CancellationTokenSource cts)
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

            await SendRequest(method, path, host, stream, cts.Token, Encoding.UTF8.GetBytes(body));
            await ReceiveResponse(stream, cts.Token);
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Erreur dans le format de l'url");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static async Task SendRequest(string method, string path, string host, NetworkStream stream,
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
        await stream.WriteAsync(byteRequest, ct);
    }

    public static async Task ReceiveResponse(NetworkStream stream, CancellationToken ct)
    {
        string raw = await Utils.ReadHttpHeadersAsync(stream, ct);
        Console.WriteLine("--- Réponse du serveur ---");
        Console.WriteLine(raw);
    }
}