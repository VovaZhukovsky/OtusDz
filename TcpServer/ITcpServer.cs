namespace TcpServer;

public interface ITcpServer
{
    Task StartAsync(CancellationToken cancellationToken = default);
}