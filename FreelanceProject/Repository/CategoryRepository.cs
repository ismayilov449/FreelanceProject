using Core.Models;
using Repository.Cqrs.Commands.CategoryCommand;
using Repository.Cqrs.Queries.CategoryQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ICategoryRepository : ICategoryCommand, ICategoryQuery
    {

    }

    public class CategoryRepository : ICategoryRepository
    {
        private readonly ICategoryCommand _categoryCommand;
        private readonly ICategoryQuery _categoryQuery;

        public CategoryRepository(ICategoryCommand categoryCommand, ICategoryQuery categoryQuery)
        {
            _categoryCommand = categoryCommand;
            _categoryQuery = categoryQuery;
        }

        public async Task<Guid> Add(Category category)
        {
            var result = await _categoryCommand.Add(category);
            return result;
        }

        public async Task Delete(string id)
        {
            await _categoryCommand.Delete(id);
        }

        public async Task<ListResult<Category>> GetAll(int offset, int limit)
        {
            var result = await _categoryQuery.GetAll(offset,limit);
            return result;
        }

        public async Task<Category> GetById(string id)
        {
            var result = await _categoryQuery.GetById(id);
            return result;
        }

        public async Task Update(Category category)
        {
            await _categoryCommand.Update(category);
        }
    }
}
