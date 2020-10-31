using Core.Models;
using Dapper;
using Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Cqrs.Commands.CategoryCommand
{
    public interface ICategoryCommand
    {
        Task<Guid> Add(Category category);
        Task Delete(string id);
        Task Update(Category category);
    }

    public class CategoryCommand : ICategoryCommand
    {
        public IUnitOfWork _unitOfWork { get; set; }

        public CategoryCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string addSql = $@"Insert Into Categories(Name,ParentCategoryId,DeleteStatus)
                                   Output Inserted.Id
                                   Values (@{nameof(Category.Name)},@{nameof(Category.ParentCategoryId)},0)";

        private string deleteSql = $@"Update Categories Set DeleteStatus = 1 Where Id = @id";

        private string updateSql = $@"Update Categories Set 
                                      Name = @{nameof(Category.Name)},
                                      ParentCategoryId = @{nameof(Category.ParentCategoryId)} 
                                      Where Id = @{nameof(Category.Id)}";


        public async Task<Guid> Add(Category category)
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<Guid>(addSql, category, _unitOfWork.GetTransaction());

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

        public async Task Update(Category category)
        {
            try
            {
                await _unitOfWork.GetConnection().QueryAsync(updateSql, category, _unitOfWork.GetTransaction());
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
