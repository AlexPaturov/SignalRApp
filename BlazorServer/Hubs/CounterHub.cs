﻿using Microsoft.AspNetCore.SignalR;

namespace BlazorServer.Hubs;

public class CounterHub : Hub
{

    public Task AddToTotal(string user, int value) 
    {
        return Clients.All.SendAsync("CounterIncrement", user, value);
    }
}
