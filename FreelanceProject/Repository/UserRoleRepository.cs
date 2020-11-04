using Core.Models;
using Repository.Cqrs.Commands.AuthCommand;
using Repository.Cqrs.Queries.AuthQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUserRoleRepository : IUserRoleCommand, IUserRoleQuery
    {

    }

    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IUserRoleCommand _userRoleCommand;
        private readonly IUserRoleQuery _userRoleQuery;

        public UserRoleRepository(IUserRoleCommand userRoleCommand, IUserRoleQuery userRoleQuery)
        {
            _userRoleCommand = userRoleCommand;
            _userRoleQuery = userRoleQuery;
        }

        public async Task<Guid> Add(UserRole userRole)
        {
            var result = await _userRoleCommand.Add(userRole);
            return result;
        }

        public async Task Delete(string id)
        {
            await _userRoleCommand.Delete(id);
        }

        public async Task<IEnumerable<Role>> GetRolesByUserId(string userId)
        {
            var result = await _userRoleQuery.GetRolesByUserId(userId);
            return result;
        }

        public async Task<IEnumerable<User>> GetUsersByRoleName(string roleName)
        {
            var result = await _userRoleQuery.GetUsersByRoleName(roleName);
            return result;
        }

        public async Task<bool> IsInRole(string userId, string roleName)
        {
            var result = await _userRoleQuery.IsInRole(userId, roleName);
            return result;
        }

        public async Task Update(UserRole userRole)
        {
            await _userRoleCommand.Update(userRole);
        }
    }
}
