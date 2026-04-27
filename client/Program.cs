using System.Net;
using System.Text;

namespace client;

using System.Net.Sockets;
using shared;

public class Client
{
    public static async Task Main()
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken ct = cts.Token;
        TcpClient client = new TcpClient();
        NetworkStream stream = client.GetStream();

        while (true)
        {
            await HandleClient(cts, stream);
        }
    }

    public static async Task HandleClient(CancellationTokenSource cts, NetworkStream stream)
    {
        Console.WriteLine("--- Nouvelle Requête ---");
        Console.Write("Entrez l'URL (ex: http://localhost:5000/api/messages) : ");
        string url = Console.ReadLine();
        
        string carved = url.Replace("http://", "");
        int slashIndex = carved.IndexOf("/");
        
        string addressAndPort = carved.Substring(0, slashIndex);
        string path = carved.Substring(slashIndex);
    
        string[] parts = addressAndPort.Split(':');
        string host = parts[0];
        string  port = parts[1];

        HttpRequest request = new HttpRequest();

        await HttpRequestWriter.WriteRequest(request);
    }
}