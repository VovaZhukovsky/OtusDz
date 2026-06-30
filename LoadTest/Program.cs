using System.Net;
using LoadTest;
using NBomber.Contracts;
using NBomber.CSharp;

var scenarion1 = Scenario.Create("set to tcp-server", async context =>
    {
        using var tcpClient = new TcpTestClient();
        await tcpClient.ConnectAsync(IPAddress.Loopback, 8080);
        var response = await tcpClient.SetAsync($"user:{RandomInt()}", $"data:{RandomInt()}");
        if (response == "OK")
            return Response.Ok();

        return Response.Fail();
    })
    .WithWarmUpDuration(TimeSpan.FromSeconds(10))
    .WithLoadSimulations(
        LoadSimulation.NewInject(_rate: 100, _during: TimeSpan.FromSeconds(30), _interval: TimeSpan.FromSeconds(1))
    );

var scenarion2 = Scenario.Create("get from tcp-server", async context =>
    {
        using var tcpClient = new TcpTestClient();
        await tcpClient.ConnectAsync(IPAddress.Loopback, 8080);
        var response = await tcpClient.GetAsync($"user:{RandomInt()}");
        if (response != "(nil)")
            return Response.Ok();

        return Response.Fail();
    })
    .WithWarmUpDuration(TimeSpan.FromSeconds(10))
    .WithLoadSimulations(
        LoadSimulation.NewInject(_rate: 100, _during: TimeSpan.FromSeconds(30), _interval: TimeSpan.FromSeconds(1))
    );
var scenarion3 = Scenario.Create("remove from tcp-server", async context =>
    {
        using var tcpClient = new TcpTestClient();
        await tcpClient.ConnectAsync(IPAddress.Loopback, 8080);
        var response = await tcpClient.DeleteAsync($"user:{RandomInt()}");
        if (response == "OK")
            return Response.Ok();

        return Response.Fail();
    })
    .WithWarmUpDuration(TimeSpan.FromSeconds(10))
    .WithLoadSimulations(
        LoadSimulation.NewInject(_rate: 100, _during: TimeSpan.FromSeconds(30), _interval: TimeSpan.FromSeconds(1))
    );

NBomberRunner
    .RegisterScenarios(scenarion1, scenarion2, scenarion3)
    .Run();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
string RandomSimbols()
{
    return Guid.NewGuid().ToString().Split("-")[0];
}

string RandomInt()
{
    var random = new Random();
    return random.Next(0, 100).ToString();
}