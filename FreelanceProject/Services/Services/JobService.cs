using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Models.SearchModels;
using Repository;

namespace Services.Services
{
    public interface IJobService
    {
        Task<Job> GetById(string id);
        Task<ListResult<Job>> GetAll(int offset, int limit);
        Task<ListResult<Job>> GetFullSearch(JobSearchModel jobSearchModel);
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

        public async Task<ListResult<Job>> GetAll(int offset, int limit)
        {
            var result = await _jobRepository.GetAll(offset, limit);
            return result;
        }

        public async Task<Job> GetById(string id)
        {
            var result = await _jobRepository.GetById(id);
            return result;
        }

        public async Task<ListResult<Job>> GetFullSearch(JobSearchModel jobSearchModel)
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
