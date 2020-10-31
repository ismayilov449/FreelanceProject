using Core.Models;
using Dapper;
using Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Cqrs.Commands.JobCommand
{
    public interface IJobCommand
    {
        Task<Guid> Add(Job job);
        Task Delete(string id);
        Task Update(Job job);
    }

    public class JobCommand : IJobCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string addSql = $@"Insert Into Jobs(RecruiterId,
CategoryId,
CityId,
EducationId,
Experience,
Position,
CompanyName,
SalaryMin,
SalaryMax,
AgeMin,
AgeMax,
Requirements,
Description,
PublishedDate,
EndDate,
Deadline,
DeleteStatus,
PremiumStatus,
ApproveStatus)
Values(@{nameof(Job.RecruiterId)},
@{nameof(Job.CategoryId)},
@{nameof(Job.CityId)},
@{nameof(Job.EducationId)},
@{nameof(Job.Experience)},
@{nameof(Job.Position)},
@{nameof(Job.CompanyName)},
@{nameof(Job.SalaryMin)},
@{nameof(Job.SalaryMax)},
@{nameof(Job.AgeMin)},
@{nameof(Job.AgeMax)},
@{nameof(Job.Requirements)},
@{nameof(Job.Description)},
@{nameof(Job.PublishedDate)},
@{nameof(Job.EndDate)},
@{nameof(Job.Deadline)},
0,
@{nameof(Job.PremiumStatus)},
@{nameof(Job.ApproveStatus)})";

        private string deleteSql = $@"Update Jobs Set DeleteStatus = 1 Where Id = @id";

        private string updateSql = $@"Update Jobs Set 
RecruiterId = @{nameof(Job.RecruiterId)},
CategoryId = @{nameof(Job.CategoryId)},
CityId = @{nameof(Job.CityId)},
EducationId = @{nameof(Job.EducationId)},
Experience = @{nameof(Job.Experience)},
Position = @{nameof(Job.Position)},
CompanyName = @{nameof(Job.CompanyName)},
SalaryMin = @{nameof(Job.SalaryMin)},
SalaryMax = @{nameof(Job.SalaryMax)},
AgeMin = @{nameof(Job.AgeMin)},
AgeMax = @{nameof(Job.AgeMax)},
Requirements = @{nameof(Job.Requirements)},
Description = @{nameof(Job.Description)},
PublishedDate = @{nameof(Job.PublishedDate)},
EndDate = @{nameof(Job.EndDate)},
Deadline = @{nameof(Job.Deadline)},
PremiumStatus = @{nameof(Job.PremiumStatus)},
ApproveStatus = @{nameof(Job.ApproveStatus)}
";

        public async Task<Guid> Add(Job job)
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<Guid>(addSql, job, _unitOfWork.GetTransaction());
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(string id)
        {
            try
            {
                await _unitOfWork.GetConnection().QueryAsync(deleteSql, new { id }, _unitOfWork.GetTransaction());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Update(Job job)
        {
            try
            {
                await _unitOfWork.GetConnection().QueryAsync(updateSql, job, _unitOfWork.GetTransaction());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
