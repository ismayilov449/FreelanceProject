using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Dapper;
using Repository.Infrastructure;

namespace Repository.Cqrs.Queries.CityQuery
{
    public interface ICtyQuery
    {
        Task<IEnumerable<City>> GetAll();
        Task<City> GetById(string id);
    }
    public class CtyQuery : ICtyQuery
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public CtyQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private string getAllSql = @"Select * from Cities where DeleteStatus=0";
        private string getByIdSql = @"Select * from Cities
                                      Where Id = @id";
        public async Task<IEnumerable<City>> GetAll()
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryAsync<City>(getAllSql, null, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<City> GetById(string id)
        {
            var param = new
            {
                id
            };
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<City>(getByIdSql, param, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
