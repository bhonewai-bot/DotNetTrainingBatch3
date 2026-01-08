using Microsoft.AspNetCore.SignalR;

namespace DotNetTrainingBatch3.SignalRchat2.Hubs;

public class ChatHub : Hub
{
    public async Task ServerReceiveMessage(string user, string message)
    {
        await Clients.All.SendAsync("ClientReceiveMessage", user, message);
    }
}