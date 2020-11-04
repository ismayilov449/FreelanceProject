using Core.Models;
using Dapper;
using Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Cqrs.Queries.AuthQuery
{
    public interface IRoleQuery
    {
        Task<Role> GetById(string id);
        Task<IEnumerable<Role>> GetAll();
    }

    public class RoleQuery : IRoleQuery
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string getAllSql = $@"Select * From Roles Where DeleteStatus = 0";
        private string getByIdSql = $@"Select * From Roles Where Id = @id and DeleteStatus = 0";

        public async Task<Role> GetById(string id)
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<Role>(getByIdSql, new { id }, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryAsync<Role>(getAllSql, null, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
