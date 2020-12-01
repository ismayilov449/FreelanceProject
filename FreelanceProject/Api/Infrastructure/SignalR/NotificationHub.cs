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
        private readonly IClientManager _clientManager;

        public NotificationHub(IClientManager clientManager)
        {
            _clientManager = clientManager;
        }

        public async Task SendNotification(Job job)
        {
            var connectionIds = _clientManager.GetAllClients();

            await Clients.All.SendAsync("ReceiveNotification", job);
        }

        public async Task RefreshNotification(Job job, string userId)
        {
            var clients = _clientManager.GetAllClients();

            var connectionIds = clients.Where(x => x.ConnectionId != Context.ConnectionId && x.UserId == userId).Select(x => x.ConnectionId).ToList();

            await Clients.Clients(connectionIds).SendAsync("ReceiveNotification", job);
        }

        public async override Task OnConnectedAsync()
        {
            try
            {
                var userId = Context.GetHttpContext().Request.Query["userId"];

                if (userId.Count > 0)
                {
                    _clientManager.AddClient(new SocketClient
                    {
                        ConnectionId = Context.ConnectionId,
                        Id = Guid.NewGuid().ToString(),
                        UserId = userId,

                    });
                }

                //await Clients.AddToGroupAsync(Context.ConnectionId, userId);
            }
            catch (Exception e)
            {

            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _clientManager.RemoveClient(new SocketClient
            {
                ConnectionId = Context.ConnectionId
            });
            await base.OnDisconnectedAsync(exception);
        }
    }
}
