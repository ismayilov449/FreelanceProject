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
    }

    public class NotifyService : INotifyService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotifyService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotification(Job job)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", job);
        }
    }
}
