using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class UserRole
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
