using System.Buffers;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ZeroAllocCore;

namespace TcpServer;

public class TcpServer : ITcpServer, IDisposable
{
    private bool _isDisposed;
    private readonly List<Socket> _clientSockets = new();
    private readonly SimpleStore _store;
    private int Port { get; }

    public TcpServer(int port, SimpleStore store)
    {
        Port = port;
        _store = store;
    }
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(new IPEndPoint(IPAddress.Loopback, Port));
        socket.Listen();
       
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break; 
            }
            
            var clientSocket = await socket.AcceptAsync(cancellationToken);
            _clientSockets.Add(clientSocket);
            _ = Task.Run(() => ProcessClientAsync(clientSocket, cancellationToken), cancellationToken);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isManual)
    {
        if (_isDisposed)
            return;
        
        if (isManual)
        {
            foreach (var clientSocket in _clientSockets)
            {
                clientSocket.Dispose();
            }
        }

        _isDisposed = true;
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
                var result = ParserResponse(CommandParser.Parse(response.Span));
                await clientSocket.SendAsync(result);

            }
        }
        finally
        {
            DisposeClientSocket(clientSocket);
            pool.Return(buffer);
        }
    }

    private byte[] ParserResponse(CommandParserResponse response)
    {
        var key = response.Key.ToString();
        var result = "OK\r\n";
        switch (response.Command.ToString().ToLowerInvariant())
        {
            case "set":
                _store.Set(key, Encoding.UTF8.GetBytes(response.Value.ToArray()));
                break;
            case "delete":
                _store.Remove(key);
                break;
            case "get":
                var value = _store.Get(key);
                if (value == null)
                {
                    result = "(nil)\r\n";
                    break;
                }
                result = $"{Encoding.UTF8.GetString(value)}\r\n";
                break;
            default:
                result = "-ERR Unknown command\r\n";
                break;
        }
        return Encoding.UTF8.GetBytes(result);
    }
    private void WriteToConsole(CommandParserResponse response)
    {
        Console.WriteLine($"Command: {response.Command}, Key: {response.Key}, Value: {response.Value}");
    }
    
    private void DisposeClientSocket(Socket clientSocket)
    {
        clientSocket.Shutdown(SocketShutdown.Receive);
        clientSocket.Close();
    }
    
    ~TcpServer()
    {
        Dispose(false);
    }
}