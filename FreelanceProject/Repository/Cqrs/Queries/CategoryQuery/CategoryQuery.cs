using Core.Models;
using Dapper;
using Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Cqrs.Queries.CategoryQuery
{
    public interface ICategoryQuery
    {
        Task<ListResult<Category>> GetAll(int offset , int limit);
        Task<Category> GetById(string id);
    }

    public class CategoryQuery : ICategoryQuery
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public CategoryQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private string getAllSql = @"Select * from Categories where DeleteStatus=0
ORDER BY Name DESC OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY
SELECT COUNT(Id) TOTALCOUNT From Categories Where DeleteStatus=0";
        private string getByIdSql = @"Select * from Categories
                                      Where Id = @id";

        public async Task<ListResult<Category>> GetAll(int offset, int limit)
        {
            var param = new
            {
                Offset = offset,
                Limit = limit
            };
            try
            {
                var data = await _unitOfWork.GetConnection().QueryMultipleAsync(getAllSql, param, _unitOfWork.GetTransaction());
                var result = new ListResult<Category>
                {
                    List = data.Read<Category>(),
                    TotalCount = data.ReadFirst<int>()
                };
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Category> GetById(string id)
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<Category>(getByIdSql, new { id }, _unitOfWork.GetTransaction());

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
