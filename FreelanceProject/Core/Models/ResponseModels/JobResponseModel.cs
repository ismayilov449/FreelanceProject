using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.ResponseModels
{
    public class JobResponseModel : Job
    {
        public Guid Id { get; set; }
        public Guid RecruiterId { get; set; }
        public string Username { get; set; }
        public Guid CategoryId { get; set; }
        public string Category { get; set; }
        public Guid CityId { get; set; }
        public string City { get; set; }
        public Guid EducationId { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string Position { get; set; }
        public string CompanyName { get; set; }
        public decimal SalaryMin { get; set; }
        public decimal SalaryMax { get; set; }
        public short AgeMin { get; set; }
        public short AgeMax { get; set; }
        public string Requirements { get; set; }
        public string Description { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Deadline { get; set; }
        public bool DeleteStatus { get; set; }
        public bool PremiumStatus { get; set; }
        public ApproveStatus ApproveStatus { get; set; }
    }
}
