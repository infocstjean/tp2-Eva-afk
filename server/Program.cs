using System.Net;
using System.Text;
namespace server;

using System.Net.Sockets;
using shared;

public class Server
{
    public static async Task Main()
    {
        Dictionary<int, string> messages = new Dictionary<int, string>();
        TcpListener listener = new TcpListener(IPAddress.Loopback, 8088);
        listener.Start();
        CancellationTokenSource cts = new CancellationTokenSource();
        Console.Write("Départ du serveur, écire exit pour quitter : ");

        Task serverTask = HandleServer(cts.Token, listener, messages);
        await Task.Run(() =>
        {
            while (true)
            {
                string? req = Console.ReadLine();
                if (req == "quit" || req == "exit")
                {
                    cts.Cancel();
                    Console.WriteLine("Fermeture du serveur...");
                    break;
                }
            }
        });
    }

    public static async Task HandleServer(CancellationToken ct, TcpListener listener, Dictionary<int, string> messages)
    {
        while (!ct.IsCancellationRequested)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            NetworkStream stream = client.GetStream();
            string raw = await ReadHttpHeadersAsync(stream);
            HttpRequest request = HttpRequestParser.ParseRequest(raw);
            HttpResponse response = Router.Handle(request, messages);
            await stream.WriteAsync(HttpResponseWriter.ToBytes(response));
            client.Close();
            stream.Close();
        }
    }

    static async Task<string> ReadHttpHeadersAsync(NetworkStream stream)
    {
        byte[] buffer = new byte[1024];
        using var ms = new MemoryStream();
        while (true)
        {
            int n = await stream.ReadAsync(buffer, 0, buffer.Length);
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