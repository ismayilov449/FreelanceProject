using Core.Models;
using Dapper;
using Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Cqrs.Queries.AuthQuery
{
    public interface IUserRoleQuery
    {
        Task<IEnumerable<User>> GetUsersByRoleName(string roleName);
        Task<IEnumerable<Role>> GetRolesByUserId(string userId);
        Task<bool> IsInRole(string userId, string roleName);
    }

    public class UserRoleQuery : IUserRoleQuery
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRoleQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string getUsersByRoleNameSql = $@"Select Distinct U.* From UserRoles UR
Left Join Users U On UR.UserId = U.Id
Left Join Roles R on UR.RoleId = R.Id
Where R.Name LIKE @roleName";

        private string getRolesByUserIdSql = $@"Select R.* From UserRoles UR
LEFT JOIN Users U on UR.UserId = U.Id
LEFT JOIN Roles R on UR.RoleId = R.Id
WHERE U.Id = @userId";

        private string isInRoleSql = $@"SELECT CASE WHEN EXISTS(
Select Distinct U.* From UserRoles UR
Left Join Users U On UR.UserId = U.Id
Left Join Roles R On UR.RoleId = R.Id
Where R.Name LIKE @roleName And U.Id = @userId
)
THEN CAST(1 AS BIT)
ELSE CAST(0 AS BIT) END";



        public async Task<IEnumerable<User>> GetUsersByRoleName(string roleName)
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryAsync<User>(getUsersByRoleNameSql, new { roleName }, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Role>> GetRolesByUserId(string userId)
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryAsync<Role>(getRolesByUserIdSql, new { userId }, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsInRole(string userId, string roleName)
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<bool>(isInRoleSql, new { userId, roleName }, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
