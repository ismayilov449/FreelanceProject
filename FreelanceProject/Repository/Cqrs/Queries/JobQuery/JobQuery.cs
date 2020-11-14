using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Models.SearchModels;
using Dapper;
using Repository.Infrastructure;

namespace Repository.Cqrs.Queries.JobQuery
{
    public interface IJobQuery
    {
        Task<ListResult<Job>> GetAll(int offset, int limit);
        Task<ListResult<Job>> GetFullSearch(JobSearchModel jobSearchModel);
        Task<Job> GetById(string id);
    }
    public class JobQuery : IJobQuery
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public JobQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private string getAllSql = @"Select * from Jobs where DeleteStatus=0 
                                    ORDER BY SalaryMin DESC OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY
                                    SELECT COUNT(Id) TOTALCOUNT From Jobs Where DeleteStatus = 0";
        private string getByIdSql = @"Select * from Jobs
                                      Where Id = @id";


        private string getFullSearch = @"select J.*,U.UserName,CIT.Name,CAT.Name,EDU.Name
from Jobs J
left join Users U ON J.RecruiterId = U.Id
left join Cities CIT on J.CityId = CIT.Id
left join Categories CAT on J.CategoryId = CAT.Id
left join Education EDU on J.EducationId = EDU.Id
";

        private string condition = @$"Where J.DeleteStatus = 0 ";
        public async Task<ListResult<Job>> GetAll(int offset, int limit)
        {
            var param = new
            {
                Offset = offset,
                Limit = limit
            };
            try
            {
                var data = await _unitOfWork.GetConnection().QueryMultipleAsync(getAllSql, param, _unitOfWork.GetTransaction());
                var result = new ListResult<Job>
                {
                    List = data.Read<Job>(),
                    TotalCount = data.ReadFirst<int>()
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Job> GetById(string id)
        {
            var param = new
            {
                id
            };
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<Job>(getByIdSql, param, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ListResult<Job>> GetFullSearch(JobSearchModel jobSearchModel)
        {
            if (!String.IsNullOrWhiteSpace(jobSearchModel.Category))
            {
                condition += $@"AND CAT.Name Like '%'+@Category+'%' ";
            }
            if (!String.IsNullOrWhiteSpace(jobSearchModel.City))
            {
                condition += $@"AND CIT.Name Like '%'+@City+'%' ";
            }
            if (!String.IsNullOrWhiteSpace(jobSearchModel.Education))
            {
                condition += $@"AND EDU.Name Like '%'+@Education+'%' ";
            }
            if(jobSearchModel.Salary != 0)
            {
                condition += $@"AND J.SalaryMin < @Salary AND J.SalaryMax > @Salary ";
            }

            getFullSearch += condition;

            var paging = @"ORDER BY J.SalaryMin DESC OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY
SELECT COUNT(J.Id) TOTALCOUNT
from Jobs J
left join Cities CIT on J.CityId = CIT.Id
left join Categories CAT on J.CategoryId = CAT.Id
left join Education EDU on J.EducationId = EDU.Id
                               " + condition;

            getFullSearch += paging;

            var param = new
            {

                City = jobSearchModel.City,
                Education = jobSearchModel.Education,
                Category = jobSearchModel.Category,
                Salary = jobSearchModel.Salary,
                Offset = jobSearchModel.Offset,
                Limit = jobSearchModel.Limit

            };

            try
            {
                var data = await _unitOfWork.GetConnection().QueryMultipleAsync(getFullSearch, param, _unitOfWork.GetTransaction());

                var result = new ListResult<Job>
                {
                    List = data.Read<Job>(),
                    TotalCount = data.ReadFirst<int>()
                };
                return result;

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
