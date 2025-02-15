namespace DataCat.Server.Metrics.Hubs;

public class MetricsHub() : Hub
{
    public async Task Send(string name, string message)
    {
        Console.WriteLine($"[{Context.ConnectionId}] {name}: {message}");
        await Clients.All.SendAsync("broadcastMessage", name, message);
    }
}