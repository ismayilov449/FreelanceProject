using Core.Models;
using Dapper;
using Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Cqrs.Commands.AuthCommand
{
    public interface IUserRoleCommand
    {
        Task<Guid> Add(UserRole userRole);
        Task Update(UserRole userRole);
        Task Delete(string id);
    }

    public class UserRoleCommand : IUserRoleCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRoleCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string addSql = $@"Insert Into UserRoles(UserId,RoleId,DeleteStatus)
                                   Values(@{nameof(UserRole.UserId)},@{nameof(UserRole.RoleId)},0)";

        private string deleteSql = $@"Update UserRoles Set DeleteStatus = 1 Where Id = @id";

        private string updateSql = $@"Update UserRoles Set UserId = @{nameof(UserRole.UserId)},RoleId = @{nameof(UserRole.RoleId)} Where Id = @{nameof(UserRole.Id)}";

        public async Task<Guid> Add(UserRole userRole)
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<Guid>(addSql, userRole, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(string id)
        {
            try
            {
                await _unitOfWork.GetConnection().QueryAsync(deleteSql, new { id }, _unitOfWork.GetTransaction());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //it doesnt work
        public async Task Update(UserRole userRole)
        {
            try
            {
                await _unitOfWork.GetConnection().QueryAsync(updateSql, userRole, _unitOfWork.GetTransaction());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
