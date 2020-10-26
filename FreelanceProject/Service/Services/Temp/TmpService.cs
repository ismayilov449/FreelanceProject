using Core.temp_folder;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Temp
{
    public interface ITmpService
    {
        Task<IEnumerable<TempClass>> GetAll();
        Task<TempClass> GetById(int id);
        Task<int> Add(TempClass entity);
    }
    public class TmpService : ITmpService
    {
        private readonly ITempRepository _tempRepository;

        public TmpService(ITempRepository tempRepository)
        {
            _tempRepository = tempRepository;
        }

        public Task<int> Add(TempClass entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TempClass>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<TempClass> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
