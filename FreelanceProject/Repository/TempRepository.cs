using Core.temp_folder;
using Repository.Cqrs.Commands.TempCommand;
using Repository.Cqrs.Queries.TempQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ITempRepository : ITmpCommand, ITmpQuery
    {
    }

    public class TempRepository : ITempRepository
    {
        private readonly ITmpCommand _tmpCommand;
        private readonly ITmpQuery _tmpQuery;

        public TempRepository(ITmpCommand tmpCommand, ITmpQuery tmpQuery)
        {
            _tmpCommand = tmpCommand;
            _tmpQuery = tmpQuery;
        }

        public async Task<int> Add(TempClass entity)
        {
            var result = await _tmpCommand.Add(entity);
            return result;
        }

        public async Task<IEnumerable<TempClass>> GetAll()
        {
            var result = await _tmpQuery.GetAll();
            return result;
        }

        public async Task<TempClass> GetById(int id)
        {
            var result = await _tmpQuery.GetById(id);
            return result;
        }
    }
}
