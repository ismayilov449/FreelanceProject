using Core.Models;
using Dapper;
using Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Cqrs.Queries.AuthQuery
{
    public interface IUserQuery
    {
        Task<User> GetById(string id);
        Task<User> GetByEmail(string email);
        Task<User> GetByUsername(string userName);
        Task<bool> Exists(string id);
        Task<IEnumerable<User>> GetAll();
    }

    public class UserQuery : IUserQuery
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string getByIdSql = $@"Select * From Users Where Id = @id And DeleteStatus = 0";

        private string getByEmailSql = $@"Select * From Users Where Email Like @email And DeleteStatus = 0";

        private string getByUsernameSql = $@"Select * From Users Where UserName Like @username and DeleteStatus = 0";

        private string getAllSql = $@"Select * From Users Where DeleteStatus = 0";

        private string existsSql = $@"Select Case When Exists (
Select * From Users U
Where U.Id = @id
)
Then Cast(1 AS BIT)
Else Cast(0 AS BIT) End";

        public async Task<User> GetById(string id)
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<User>(getByIdSql, new { id }, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<User> GetByEmail(string email)
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<User>(getByEmailSql, new { email }, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<User> GetByUsername(string userName)
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<User>(getByUsernameSql, new { userName }, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryAsync<User>(getAllSql, null, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Exists(string id)
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<bool>(existsSql, new { id }, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
