using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Models
{
    public class ListResult<T> where T : class
    {
        public IEnumerable<T> List { get; set; }
        public int TotalCount { get; set; }
    }
}
