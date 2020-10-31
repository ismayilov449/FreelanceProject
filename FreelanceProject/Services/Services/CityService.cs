using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Repository;

namespace Services.Services
{
    public interface ICityService
    {
        Task<City> GetById(string id);
        Task<IEnumerable<City>> GetAll();
        Task<Guid> Add(City entity);
        Task Delete(string id);
        Task Update(City entity);

    }

    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;

        public CityService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<Guid> Add(City entity)
        {
            var result = await _cityRepository.Add(entity);
            return result;
        }

        public async Task Delete(string id)
        {
            await _cityRepository.Delete(id);
        }

        public async Task<IEnumerable<City>> GetAll()
        {
            var result = await _cityRepository.GetAll();
            return result;
        }

        public async Task<City> GetById(string id)
        {
            var result = await _cityRepository.GetById(id);
            return result;
        }

        public async Task Update(City entity)
        {
            await _cityRepository.Update(entity);
        }
    }
}
