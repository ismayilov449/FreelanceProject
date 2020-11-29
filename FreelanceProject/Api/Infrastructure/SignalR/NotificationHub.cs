using Core.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Infrastructure.SignalR
{
    public class NotificationHub : Hub
    {

        public async Task SendNotification(Job job)
        {
            await Clients.All.SendAsync("ReceiveNotification", job);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Connected", Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("DisConnected", Context.ConnectionId);
        }
    }
}
