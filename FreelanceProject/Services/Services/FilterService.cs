using Core.Models.ServiceModels;
using Repository;
using Repository.Cqrs.Queries.FilterQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IFilterService
    {
        Task<Guid> SubscribeFilter(Filter filter, string userId);
        Task<IEnumerable<TempUser>> GetUsers(FilterRequestModel filterRequestModel);
        Task Delete(string id);

    }
    public class FilterService : IFilterService
    {
        private readonly IFilterRepository _filterRepository;

        public FilterService(IFilterRepository filterRepository)
        {
            _filterRepository = filterRepository;
        }

        public async Task<IEnumerable<TempUser>> GetUsers(FilterRequestModel filterRequestModel)
        {
            var result = await _filterRepository.GetUsers(filterRequestModel);
            return result;
        }

        public async Task<Guid> SubscribeFilter(Filter filter, string userId)
        {
           return await _filterRepository.SubscribeFilter(filter, userId);
        }
        public async Task Delete(string id)
        {
            await _filterRepository.Delete(id);
        }
    }
}
