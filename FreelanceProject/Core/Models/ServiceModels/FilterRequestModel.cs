using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.ServiceModels
{
    public class FilterRequestModel
    {
        public string CategoryId { get; set; }
        public string CityId { get; set; }
        public string EducationId { get; set; }
        public int Salary { get; set; }
        public string UserId { get; set; }
    }
}
