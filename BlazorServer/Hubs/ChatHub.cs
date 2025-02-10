using Microsoft.AspNetCore.SignalR;

namespace BlazorServer.Hubs;

// это у нас типа сервера
public class ChatHub :Hub
{
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public Task SendMessage(string user, string message) 
    {
        return Clients.All.SendAsync("ReceiveMessage", user, message);    
    }
}
