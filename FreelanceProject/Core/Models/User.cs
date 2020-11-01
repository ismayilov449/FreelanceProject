using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool DeleteStatus { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
