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