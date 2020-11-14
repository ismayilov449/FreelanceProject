using Core.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface ICategoryService
    {
        Task<Category> GetById(string id);
        Task<ListResult<Category>> GetAll(int offset,int limit);
        Task<Guid> Add(Category category);
        Task Delete(string id);
        Task Update(Category category);
    }
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Guid> Add(Category category)
        {
            var result = await _categoryRepository.Add(category);
            return result;
        }

        public async Task Delete(string id)
        {
            await _categoryRepository.Delete(id);
        }

        public async Task<ListResult<Category>> GetAll(int offset,int limit)
        {
            var result = await _categoryRepository.GetAll(offset,limit);
            return result;
        }

        public async Task<Category> GetById(string id)
        {
            var result = await _categoryRepository.GetById(id);
            return result;
        }

        public async Task Update(Category category)
        {
            await _categoryRepository.Update(category);
        }
    }
}
