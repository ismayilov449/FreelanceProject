using Core.Models.ServiceModels;
using Repository.Cqrs.Commands.FilterCommand;
using Repository.Cqrs.Queries.FilterQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IFilterRepository
    {
        Task<Guid> SubscribeFilter(Filter filter, string userId);
        Task<IEnumerable<TempUser>> GetUsers(FilterRequestModel filterRequestModel);

        Task Delete(string id);
    }

    public class FilterRepository : IFilterRepository
    {
        private readonly IFilterCommand _filterCommand;
        private readonly IFilterQuery _filterQuery;

        public FilterRepository(IFilterCommand filterCommand, IFilterQuery filterQuery)
        {
            _filterCommand = filterCommand;
            _filterQuery = filterQuery;
        }

        public async Task<IEnumerable<TempUser>> GetUsers(FilterRequestModel filterRequestModel)
        {
            var result = await _filterQuery.GetUsers(filterRequestModel);
            return result;
        }

        public async Task<Guid> SubscribeFilter(Filter filter, string userId)
        {
            return await _filterCommand.SubscribeFilter(filter, userId);
        }
        public async Task Delete(string id)
        {
            await _filterCommand.Delete(id);
        }

    }

}
