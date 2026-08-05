using System.Diagnostics;using System.Diagnostics.Metrics;using System.Net;
using System.Net.Sockets;
using System.Text;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using TcpServer;
using ZeroAllocCore;



public class Program
{
    public static readonly ActivitySource ActivitySource = new(nameof(TcpServer));
    public static readonly Meter Meter = new(nameof(TcpServer));
    public static readonly Counter<long> SetCommands = 
        Meter.CreateCounter<long>("set_command_total");
    public static readonly Counter<long> GetCommands = 
        Meter.CreateCounter<long>("get_command_total");
    public static readonly Counter<long> DeleteCommands = 
        Meter.CreateCounter<long>("delete_command_total");
        
    public static readonly Histogram<double> SetCommandsHistogram = 
        Meter.CreateHistogram<double>("set_command_duration_seconds");
    public static readonly Histogram<double> GetCommandsHistogram = 
        Meter.CreateHistogram<double>("get_command_duration_seconds");
    public static readonly Histogram<double> DeleteCommandsHistogram = 
        Meter.CreateHistogram<double>("delete_command_duration_seconds");
    public static void Main(string[] args)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(nameof(TcpServer));

        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddSource($"{nameof(TcpServer)}")
            .AddConsoleExporter()
            .Build();

        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddMeter($"{nameof(TcpServer)}")
            .AddConsoleExporter()            
            .Build();

        //server
        using CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken serverToken = cts.Token;

        using var server = new TcpServer.TcpServer(8080,new SimpleStore());
        _ = server.StartAsync(serverToken);

        Console.ReadLine();

    }
}