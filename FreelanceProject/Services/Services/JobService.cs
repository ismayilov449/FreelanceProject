using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Models.ResponseModels;
using Core.Models.SearchModels;
using Repository;

namespace Services.Services
{
    public interface IJobService
    {
        Task<JobResponseModel> GetById(string id);
        Task<ListResult<JobResponseModel>> GetAll(int offset, int limit);
        Task<ListResult<JobResponseModel>> GetByCategory(string categoryId, int offset, int limit);
        Task<ListResult<JobResponseModel>> GetFullSearch(JobSearchModel jobSearchModel);
        Task<Guid> Add(Job entity);
        Task Delete(string id);
        Task Update(Job entity);

    }

    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<Guid> Add(Job entity)
        {
            var result = await _jobRepository.Add(entity);
            return result;
        }

        public async Task Delete(string id)
        {
            await _jobRepository.Delete(id);
        }

        public async Task<ListResult<JobResponseModel>> GetAll(int offset, int limit)
        {
            var result = await _jobRepository.GetAll(offset, limit);
            return result;
        }

        public async Task<ListResult<JobResponseModel>> GetByCategory(string categoryId, int offset, int limit)
        {
            var result = await _jobRepository.GetByCategory(categoryId, offset, limit);
            return result;
        }

        public async Task<JobResponseModel> GetById(string id)
        {
            var result = await _jobRepository.GetById(id);
            return result;
        }

        public async Task<ListResult<JobResponseModel>> GetFullSearch(JobSearchModel jobSearchModel)
        {
            var result = await _jobRepository.GetFullSearch(jobSearchModel);
            return result;
        }

        public async Task Update(Job entity)
        {
            await _jobRepository.Update(entity);
        }
    }
}
