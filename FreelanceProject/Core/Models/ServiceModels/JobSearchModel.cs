using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.SearchModels
{
    public class JobSearchModel
    {
        public string Category { get; set; }
        public string City { get; set; }
        public string Education { get; set; }
        public decimal Salary { get; set; }

        public int Offset { get; set; }
        public int Limit { get; set; }
    }
}
