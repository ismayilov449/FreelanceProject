using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Dapper;
using Repository.Infrastructure;

namespace Repository.Cqrs.Queries.JobQuery
{
    public interface IJobQuery
    {
        Task<IEnumerable<Job>> GetAll();
        Task<Job> GetById(string id);
    }
    public class JobQuery : IJobQuery
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public JobQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private string getAllSql = @"Select * from Jobs where DeleteStatus=0";
        private string getByIdSql = @"Select * from Jobs
                                      Where Id = @id";
        public async Task<IEnumerable<Job>> GetAll()
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryAsync<Job>(getAllSql, null, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Job> GetById(string id)
        {
            var param = new
            {
                id
            };
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<Job>(getByIdSql, param, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
