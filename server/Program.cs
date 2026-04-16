using System.Text;

namespace server;

using System.Net.Sockets;
using shared;

public class Server
{
    public static async Task Main()
    {
        HttpResponseWriter httpResponseWriter = new HttpResponseWriter();
        TcpListener listener = TcpListener.Create(3000);
        listener.Start();
        while (true)
        {
            using TcpClient client = listener.AcceptTcpClient();
            using NetworkStream stream = client.GetStream();
            string raw = ReadHttpHeaders(stream);
        }
        
    }

    static string ReadHttpHeaders(NetworkStream stream)
    {
        byte[] buffer = new byte[1024];
        using var ms = new MemoryStream();
        while (true)
        {
            int n = stream.Read(buffer, 0, buffer.Length);
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