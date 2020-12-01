using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Infrastructure.SignalR
{
    public class SocketClient
    {

        public string Id { get; set; }
        public string UserId { get; set; }
        public string ConnectionId { get; set; }

        #region Equality
        protected bool Equals(SocketClient other)
        {
            return string.Equals(this.ConnectionId, other.ConnectionId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((SocketClient)obj);
        }
        public override int GetHashCode()
        {
            return this.ConnectionId != null ? this.ConnectionId.GetHashCode() : 0;
        }
        #endregion
    }
}
