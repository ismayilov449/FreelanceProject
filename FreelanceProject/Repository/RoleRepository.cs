using Core.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using Repository.Cqrs.Commands.AuthCommand;
using Repository.Cqrs.Queries.AuthQuery;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRoleRepository : IRoleCommand, IRoleQuery
    {

    }

    public class RoleRepository : IRoleRepository
    {
        private readonly IRoleCommand _roleCommand;
        private readonly IRoleQuery _roleQuery;

        public RoleRepository(IRoleCommand roleCommand, IRoleQuery roleQuery)
        {
            _roleCommand = roleCommand;
            _roleQuery = roleQuery;
        }

        public async Task<Guid> Add(Role role)
        {
            var result = await _roleCommand.Add(role);
            return result;
        }

        public async Task Delete(string id)
        {
            await _roleCommand.Delete(id);
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            var result = await _roleQuery.GetAll();
            return result;
        }

        public async Task<Role> GetById(string id)
        {
            var result = await _roleQuery.GetById(id);
            return result;
        }

        public async Task Update(Role role)
        {
            await _roleCommand.Update(role);
        }
    }
}
