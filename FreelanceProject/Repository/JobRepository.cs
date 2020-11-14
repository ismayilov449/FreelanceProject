using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Models.SearchModels;
using Repository.Cqrs.Commands.JobCommand;
using Repository.Cqrs.Queries.JobQuery;

namespace Repository
{
    public interface IJobRepository : IJobCommand, IJobQuery
    {
    }

    public class JobRepository : IJobRepository
    {
        private readonly IJobCommand _jobCommand;
        private readonly IJobQuery _jobQuery;

        public JobRepository(IJobCommand jobCommand, IJobQuery jobQuery)
        {
            _jobCommand = jobCommand;
            _jobQuery = jobQuery;
        }

        public async Task<Guid> Add(Job entity)
        {
            var result = await _jobCommand.Add(entity);
            return result;
        }
        public async Task Delete(string id)
        {
            await _jobCommand.Delete(id);
        }
        public async Task Update(Job entity)
        {
            await _jobCommand.Update(entity);
        }
        public async Task<ListResult<Job>> GetAll(int offset, int limit)
        {
            var result = await _jobQuery.GetAll(offset, limit);
            return result;
        }

        public async Task<Job> GetById(string id)
        {
            var result = await _jobQuery.GetById(id);
            return result;
        }

        public async Task<ListResult<Job>> GetFullSearch(JobSearchModel jobSearchModel)
        {
            var result = await _jobQuery.GetFullSearch(jobSearchModel);
            return result;
        }

    }
}
