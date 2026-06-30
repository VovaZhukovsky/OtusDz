using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LoadTest;

public class TcpTestClient: ITcpClient, IDisposable
{
    private TcpClient? _client;
    private StreamReader? _reader;
    
    public async Task ConnectAsync(IPAddress host, int port)
    {
        _client = new TcpClient();
        await _client.ConnectAsync(host, port);
        var stream = _client.GetStream();
        _reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
    }

    public async Task<string?> SetAsync(string key, string value)
    {
        var message = $"set {key} {value};";
        var messageBytes = Encoding.UTF8.GetBytes(message);
        await _client.Client.SendAsync(messageBytes);
        return await _reader.ReadLineAsync();
    }

    public async Task<string?> GetAsync(string key)
    {
        var message = $"get {key};";
        var messageBytes = Encoding.UTF8.GetBytes(message);
        await _client.Client.SendAsync(messageBytes, SocketFlags.None);
        return await _reader.ReadLineAsync(); 
    }

    public async Task<string?> DeleteAsync(string key)
    {
        var message = $"delete {key};";
        var messageBytes = Encoding.UTF8.GetBytes(message);
        await _client.Client.SendAsync(messageBytes, SocketFlags.None);
        return await _reader.ReadLineAsync();
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
}