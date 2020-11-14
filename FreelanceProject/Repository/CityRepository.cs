using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Repository.Cqrs.Commands.CityCommand;
using Repository.Cqrs.Queries.CityQuery;

namespace Repository
{
    public interface ICityRepository : ICtyCommand, ICtyQuery
    {
    }

    public class CityRepository : ICityRepository
    {
        private readonly ICtyCommand _ctyCommand;
        private readonly ICtyQuery _ctyQuery;

        public CityRepository(ICtyCommand ctyCommand, ICtyQuery ctyQuery)
        {
            _ctyCommand = ctyCommand;
            _ctyQuery = ctyQuery;
        }

        public async Task<Guid> Add(City entity)
        {
            var result = await _ctyCommand.Add(entity);
            return result;
        }
        public async Task Delete(string id)
        {
            await _ctyCommand.Delete(id);
        }
        public async Task Update(City entity)
        {
            await _ctyCommand.Update(entity);
        }
        public async Task<ListResult<City>> GetAll(int offset,int limit)
        {
            var result = await _ctyQuery.GetAll(offset,limit);
            return result;
        }

        public async Task<City> GetById(string id)
        {
            var result = await _ctyQuery.GetById(id);
            return result;
        }
    }
}
