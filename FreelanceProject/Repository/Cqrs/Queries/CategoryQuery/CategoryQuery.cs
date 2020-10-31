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
        Task<IEnumerable<Category>> GetAll();
        Task<Category> GetById(string id);
    }

    public class CategoryQuery : ICategoryQuery
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public CategoryQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private string getAllSql = @"Select * from Categories where DeleteStatus=0";
        private string getByIdSql = @"Select * from Categories
                                      Where Id = @id";

        public async Task<IEnumerable<Category>> GetAll()
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryAsync<Category>(getAllSql, null, _unitOfWork.GetTransaction());

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
