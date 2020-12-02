using Core.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Infrastructure.SignalR
{
    public interface INotifyService
    {
        Task SendNotification(Job job);
        Task RefreshNotification(Job job, string userId);
    }

    public class NotifyService : INotifyService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IClientManager _clientManager;

        public NotifyService(IHubContext<NotificationHub> hubContext, IClientManager clientManager)
        {
            _hubContext = hubContext;
            _clientManager = clientManager;
        }

        public async Task RefreshNotification(Job job, string userId)
        {
            var currUserId = _clientManager.GetAllClients().FirstOrDefault(x => x.UserId == userId);

            if (currUserId != null)
            {
                await _hubContext.Clients.Client(currUserId.ConnectionId).SendAsync("ReceiveNotification", job);
            }
        }

        public async Task SendNotification(Job job)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", job);
        }
    }
}
