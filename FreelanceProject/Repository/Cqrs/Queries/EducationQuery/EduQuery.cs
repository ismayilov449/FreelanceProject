using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Dapper;
using Repository.Infrastructure;

namespace Repository.Cqrs.Queries.EducationQuery
{
    public interface IEduQuery
    {
        Task<IEnumerable<Education>> GetAll();
        Task<Education> GetById(string id);
    }
    public class EduQuery : IEduQuery
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public EduQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private string getAllSql = @"Select * from Education where DeleteStatus=0";
        private string getByIdSql = @"Select * from Education
                                      Where Id = @id";
        public async Task<IEnumerable<Education>> GetAll()
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryAsync<Education>(getAllSql, null, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Education> GetById(string id)
        {
            var param = new
            {
                id
            };
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<Education>(getByIdSql, param, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
