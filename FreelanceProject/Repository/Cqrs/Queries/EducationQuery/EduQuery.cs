using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Dapper;
using Repository.Infrastructure;

namespace Repository.Cqrs.Queries.EducationQuery
{
    public interface IEduQuery
    {
        Task<ListResult<Education>> GetAll(int offset,int limit);
        Task<Education> GetById(string id);
    }
    public class EduQuery : IEduQuery
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public EduQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private string getAllSql = @"Select * from Education where DeleteStatus=0
ORDER BY Name DESC OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY
SELECT COUNT(Id) TOTALCOUNT From Education Where DeleteStatus=0" ;
        private string getByIdSql = @"Select * from Education
                                      Where Id = @id";
        public async Task<ListResult<Education>> GetAll(int offset,int limit)
        {
            var param = new
            {
                Offset = offset,
                Limit = limit
            };
            try
            {
                var data = await _unitOfWork.GetConnection().QueryMultipleAsync(getAllSql, param, _unitOfWork.GetTransaction());
                var result = new ListResult<Education>
                {
                    List = data.Read<Education>(),
                    TotalCount = data.ReadFirst<int>()
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Education> GetById(string id)
        {
            var param = new
            {
                id
            };
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<Education>(getByIdSql, param, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
