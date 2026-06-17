using System.Net;
using System.Net.Sockets;
using System.Text;
using TcpServer;
using ZeroAllocCore;

//server
using CancellationTokenSource cts = new CancellationTokenSource();
CancellationToken serverToken = cts.Token;

using var server = new TcpServer.TcpServer(8080,new SimpleStore());
_ = server.StartAsync(serverToken);

Console.ReadLine();
/*
//client
var tasks = new List<Task>();
for (int j = 0; j < 10; j++)
{
    tasks.Add(Task.Run(() => StartClient(j)));
    Thread.Sleep(1000);
}
await Task.WhenAll(tasks);
async Task StartClient(int j)
{
    EndPoint ipEndPoint = new IPEndPoint(IPAddress.Loopback, 8080);
    using Socket client = new(
        ipEndPoint.AddressFamily, 
        SocketType.Stream, 
        ProtocolType.Tcp);

    using CancellationTokenSource cts1 = new CancellationTokenSource();
    await client.ConnectAsync(ipEndPoint);
    var i = 0;
    while (cts1.IsCancellationRequested == false)
    {
    
        if (i == 1000000)
        {
            cts1.Cancel();
            continue;
        }
        var message = $"set user:{j}-{i} data";
        var messageBytes = Encoding.UTF8.GetBytes(message);
        _ = await client.SendAsync(messageBytes, SocketFlags.None);
        i++;

    }
}
*/