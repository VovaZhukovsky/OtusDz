using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ZeroAllocCore;

namespace TcpServer;

public class TcpServer : ITcpServer
{
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(new IPEndPoint(IPAddress.Loopback, 8080));
        socket.Listen();
       
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break; 
            }
            
            var clientSocket = await socket.AcceptAsync(cancellationToken);
            _ = ProcessClientAsync(clientSocket, cancellationToken);
        }
    }

    private async Task ProcessClientAsync(Socket clientSocket, CancellationToken cancellationToken = default)
    {
        var pool = ArrayPool<byte>.Shared;
        var buffer = pool.Rent(1024);
        
        try
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                
                var received = await clientSocket.ReceiveAsync(buffer);
                
                if (received == 0)
                    break;
                
                var response = Encoding.UTF8.GetChars(buffer, 0, received).AsMemory();
                WriteToConsole(CommandParser.Parse(response.Span));
            }
        }
        finally
        {
            pool.Return(buffer);
            clientSocket.Shutdown(SocketShutdown.Receive);
            clientSocket.Close();
            clientSocket.Dispose();
        }
    }

    private void WriteToConsole(CommandParserResponse response)
    {
        Console.WriteLine($"Command: {response.Command}, Key: {response.Key}, Value: {response.Value}");
    }
}