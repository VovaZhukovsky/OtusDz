using System.Net;

namespace LoadTest;

public interface ITcpClient
{
    Task ConnectAsync(IPAddress host, int port);
    Task<string?> SetAsync(string key, string value);
    Task<string?> GetAsync(string key);
    Task<string?> DeleteAsync(string key);
}