using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Models.ResponseModels;
using Core.Models.SearchModels;
using Dapper;
using Repository.Infrastructure;

namespace Repository.Cqrs.Queries.JobQuery
{
    public interface IJobQuery
    {
        Task<ListResult<JobResponseModel>> GetAll(int offset, int limit);
        Task<ListResult<JobResponseModel>> GetFullSearch(JobSearchModel jobSearchModel);
        Task<ListResult<JobResponseModel>> GetByCategory(string categoryId, int offset, int limit);
        Task<JobResponseModel> GetById(string id);
    }
    public class JobQuery : IJobQuery
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public JobQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private string getAllSql = @"Select J.*,U.DisplayName [Username],CAT.Name Category,CIT.Name City,E.Name Education from Jobs J
left join Users U on J.RecruiterId = U.Id
left join Categories CAT on J.CategoryId = CAT.Id
left join Cities CIT on J.CityId = CIT.Id
left join Education E on J.EducationId = E.Id
where J.DeleteStatus=0
ORDER BY SalaryMin DESC OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY
SELECT COUNT(Id) TOTALCOUNT From Jobs Where DeleteStatus = 0";

        private string getByIdSql = @"Select J.*,U.DisplayName [Username],CAT.Name Category,CIT.Name City,E.Name Education from Jobs J
left join Users U on J.RecruiterId = U.Id
left join Categories CAT on J.CategoryId = CAT.Id
left join Cities CIT on J.CityId = CIT.Id
left join Education E on J.EducationId = E.Id
where J.DeleteStatus=0 and J.Id = @id";

        private string getByCategorySql = @"Select J.*,U.DisplayName [Username],CAT.Name Category,CIT.Name City,E.Name Education from Jobs J
left join Users U on J.RecruiterId = U.Id
left join Categories CAT on J.CategoryId = CAT.Id
left join Cities CIT on J.CityId = CIT.Id
left join Education E on J.EducationId = E.Id
where J.DeleteStatus=0 and CAT.Id = @id
ORDER BY SalaryMin DESC OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY
SELECT COUNT(Id) TOTALCOUNT From Jobs Where DeleteStatus = 0";

        private string getFullSearch = @"select J.*,U.UserName,CIT.Name,CAT.Name,EDU.Name
from Jobs J
left join Users U ON J.RecruiterId = U.Id
left join Cities CIT on J.CityId = CIT.Id
left join Categories CAT on J.CategoryId = CAT.Id
left join Education EDU on J.EducationId = EDU.Id
";

        private string condition = @$"Where J.DeleteStatus = 0 ";
        public async Task<ListResult<JobResponseModel>> GetAll(int offset, int limit)
        {
            var param = new
            {
                Offset = offset,
                Limit = limit
            };
            try
            {
                var data = await _unitOfWork.GetConnection().QueryMultipleAsync(getAllSql, param, _unitOfWork.GetTransaction());
                var result = new ListResult<JobResponseModel>
                {
                    List = data.Read<JobResponseModel>(),
                    TotalCount = data.ReadFirst<int>()
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JobResponseModel> GetById(string id)
        {
            var param = new
            {
                id
            };
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<JobResponseModel>(getByIdSql, param, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ListResult<JobResponseModel>> GetByCategory(string categoryId, int offset, int limit)
        {
            var param = new
            {
                Id = categoryId,
                Offset = offset,
                Limit = limit
            };

            try
            {
                var data = await _unitOfWork.GetConnection().QueryMultipleAsync(getByCategorySql, param, _unitOfWork.GetTransaction());
                var result = new ListResult<JobResponseModel>
                {
                    List = data.Read<JobResponseModel>(),
                    TotalCount = data.ReadFirst<int>()
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ListResult<JobResponseModel>> GetFullSearch(JobSearchModel jobSearchModel)
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
            if (jobSearchModel.Salary != 0)
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

                var result = new ListResult<JobResponseModel>
                {
                    List = data.Read<JobResponseModel>(),
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
