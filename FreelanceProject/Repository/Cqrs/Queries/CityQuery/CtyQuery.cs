using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Dapper;
using Repository.Infrastructure;

namespace Repository.Cqrs.Queries.CityQuery
{
    public interface ICtyQuery
    {
        Task<ListResult<City>> GetAll(int offset,int limit);
        Task<City> GetById(string id);
    }
    public class CtyQuery : ICtyQuery
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public CtyQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private string getAllSql = @"Select * from Cities where DeleteStatus=0
ORDER BY Name DESC OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY
SELECT COUNT(Id) TOTALCOUNT From Cities Where DeleteStatus=0";
        private string getByIdSql = @"Select * from Cities
                                      Where Id = @id";
        public async Task<ListResult<City>> GetAll(int offset,int limit)
        {
            var param = new
            {
                Offset = offset,
                Limit = limit
            };
            try
            {
                var data = await _unitOfWork.GetConnection().QueryMultipleAsync(getAllSql, param, _unitOfWork.GetTransaction());
                var result = new ListResult<City>
                {
                    List = data.Read<City>(),
                    TotalCount = data.ReadFirst<int>()
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<City> GetById(string id)
        {
            var param = new
            {
                id
            };
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<City>(getByIdSql, param, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
