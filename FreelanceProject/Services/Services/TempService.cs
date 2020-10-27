using Core.temp_folder;
using Repository;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface ITempService
    {
        Task<TempClass> GetById(int id);
        Task<IEnumerable<TempClass>> GetAll();
        Task<int> Add(TempClass entity);

    }

    public class TempService : ITempService
    {
        private readonly ITempRepository _tempRepository;

        public TempService(ITempRepository tempRepository)
        {
            _tempRepository = tempRepository;
        }

        public async Task<int> Add(TempClass entity)
        {
            var result = await _tempRepository.Add(entity);
            return result;
        }

        public async Task<IEnumerable<TempClass>> GetAll()
        {
            var result = await _tempRepository.GetAll();
            return result;
        }

        public async Task<TempClass> GetById(int id)
        {
            var result = await _tempRepository.GetById(id);
            return result;
        }
    }
}
