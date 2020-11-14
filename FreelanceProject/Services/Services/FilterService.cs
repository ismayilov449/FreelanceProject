using Core.Models.ServiceModels;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IFilterService
    {
        Task SubscribeFilter(Filter filter, string userId);
    }
    public class FilterService : IFilterService
    {
        private readonly IFilterRepository _filterRepository;

        public FilterService(IFilterRepository filterRepository)
        {
            _filterRepository = filterRepository;
        }

        public async Task SubscribeFilter(Filter filter, string userId)
        {
            await _filterRepository.SubscribeFilter(filter, userId);
        }
    }
}
