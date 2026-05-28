using System.Net;
using System.Net.Sockets;
using System.Text;
using TcpServer;

//server
using CancellationTokenSource cts = new CancellationTokenSource();
CancellationToken token = cts.Token;

var server = new TcpServer.TcpServer();
_ = server.StartAsync(token);


//client
EndPoint ipEndPoint = new IPEndPoint(IPAddress.Loopback, 8080);
using Socket client = new(
    ipEndPoint.AddressFamily, 
    SocketType.Stream, 
    ProtocolType.Tcp);

await client.ConnectAsync(ipEndPoint);
var i = 0;
while (cts.IsCancellationRequested == false)
{
    
    if (i == 100000000)
    {
        cts.Cancel();
        continue;
    }
    var message = $"set user:{i} data";
    var messageBytes = Encoding.UTF8.GetBytes(message);
    _ = await client.SendAsync(messageBytes, SocketFlags.None);
    i++;

}