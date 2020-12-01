using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Infrastructure.SignalR
{

    public interface IClientManager
    {
        void AddClient(SocketClient client);
        void RemoveClient(SocketClient client);
        HashSet<SocketClient> GetAllClients();
        List<SocketClient> GetClients();
        SocketClient GetClientByUserId(string userId);
        SocketClient GetClientByConnId(string connectionId);
    }

    public class ClientManager : IClientManager
    {
        private readonly HashSet<SocketClient> _clients = new HashSet<SocketClient>();
        private readonly IHubContext<NotificationHub> _hubContext;

        public ClientManager(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void AddClient(SocketClient client)
        {
            lock (_clients)
            {
                if (this._clients.Contains(client))
                {
                    this._clients.Remove(client);
                }
                this._clients.Add(client);
            }
        }

        public void RemoveClient(SocketClient client)
        {
            var t = this.GetClientByConnId(client.ConnectionId);
            lock (this._clients)
            {
                Console.WriteLine("Count Before Logout : {0}", this._clients.Count);
                this._clients.Remove(client);
                Console.WriteLine("Count After Logout : {0}", this._clients.Count);
            }
        }

        public HashSet<SocketClient> GetAllClients()
        {
            lock (this._clients)
            {
                return this._clients;
            }
        }

        public SocketClient GetClientByUserId(string userId)
        {
            lock (this._clients)
            {
                return this._clients.FirstOrDefault(x => x.UserId == userId);
                //return this._clients.Where(x => x.UserId == userId).ToList();
            }
        }

        public SocketClient GetClientByConnId(string connectionId)
        {
            var tmpObject = new SocketClient { ConnectionId = connectionId };
            lock (this._clients)
            {
                return this._clients.FirstOrDefault(x => x.ConnectionId == connectionId);
            }
        }

        public List<SocketClient> GetClients()
        {
            lock (this._clients)
            {
                return this._clients.ToList();
            }
        }
    }
}
