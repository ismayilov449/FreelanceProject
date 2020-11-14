using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.ServiceModels
{
    public class Filter
    {
        public IEnumerable<Guid> Categories { get; set; }
        public IEnumerable<Guid> Cities { get; set; }
        public IEnumerable<Guid> Education { get; set; }
        public int Salary { get; set; }

    }
}
