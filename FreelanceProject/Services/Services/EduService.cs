using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Repository;

namespace Services.Services
{
    public interface IEduService
    {
        Task<Education> GetById(string id);
        Task<IEnumerable<Education>> GetAll();
        Task<Guid> Add(Education entity);
        Task Delete(string id);
        Task Update(Education entity);

    }

    public class EduService : IEduService
    {
        private readonly IEduRepository _eduRepository;

        public EduService(IEduRepository eduRepository)
        {
            _eduRepository = eduRepository;
        }

        public async Task<Guid> Add(Education entity)
        {
            var result = await _eduRepository.Add(entity);
            return result;
        }

        public async Task Delete(string id)
        {
            await _eduRepository.Delete(id);
        }

        public async Task<IEnumerable<Education>> GetAll()
        {
            var result = await _eduRepository.GetAll();
            return result;
        }

        public async Task<Education> GetById(string id)
        {
            var result = await _eduRepository.GetById(id);
            return result;
        }

        public async Task Update(Education entity)
        {
            await _eduRepository.Update(entity);
        }
    }
}
