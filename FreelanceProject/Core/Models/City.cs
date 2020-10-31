using System;
namespace Core.Models
{
    public class City
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool DeleteStatus { get; set; }
    }
}
