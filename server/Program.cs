using System.Net;

namespace server;

using System.Net.Sockets;
using shared;

public class Server
{
    public static async Task Main()
    {
        Dictionary<int, string> messages = new Dictionary<int, string>();
        TcpListener listener = new TcpListener(IPAddress.Loopback, 8088);
        CancellationTokenSource cts = new CancellationTokenSource();
        listener.Start();
        Console.Write("Départ du serveur, écire exit pour quitter ");
        Console.Write("\n > ");

        _ = HandleServer(cts.Token, listener, messages);
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
    }

    public static async Task HandleServer(CancellationToken ct, TcpListener listener, Dictionary<int, string> messages)
    {
        while (!ct.IsCancellationRequested)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            NetworkStream stream = client.GetStream();
            string raw = await Utils.ReadHttpHeadersAsync(stream);
            Console.WriteLine("\n--- Requête du client ---");
            Console.WriteLine(raw);
            Console.Write("> ");
            HttpRequest request = HttpRequestParser.ParseRequest(raw);
            HttpResponse response = Router.Handle(request, messages);

            await stream.WriteAsync(HttpResponseWriter.ToBytes(response));
            await stream.FlushAsync();

            client.Close();
            stream.Close();
        }
    }
}