using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public bool DeleteStatus { get; set; }
    }
}
