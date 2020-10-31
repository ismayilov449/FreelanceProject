using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Repository;

namespace Services.Services
{
    public interface IJobService
    {
        Task<Job> GetById(string id);
        Task<IEnumerable<Job>> GetAll();
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

        public async Task<IEnumerable<Job>> GetAll()
        {
            var result = await _jobRepository.GetAll();
            return result;
        }

        public async Task<Job> GetById(string id)
        {
            var result = await _jobRepository.GetById(id);
            return result;
        }

        public async Task Update(Job entity)
        {
            await _jobRepository.Update(entity);
        }
    }
}
