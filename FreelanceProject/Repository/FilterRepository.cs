using Core.Models.ServiceModels;
using Repository.Cqrs.Commands.FilterCommand;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IFilterRepository
    {
        Task SubscribeFilter(Filter filter, string userId);
    }

    public class FilterRepository : IFilterRepository
    {
        private readonly IFilterCommand _filterCommand;

        public FilterRepository(IFilterCommand filterCommand)
        {
            _filterCommand = filterCommand;
        }

        public async Task SubscribeFilter(Filter filter, string userId)
        {
            await _filterCommand.SubscribeFilter(filter, userId);
        }
    }

}
