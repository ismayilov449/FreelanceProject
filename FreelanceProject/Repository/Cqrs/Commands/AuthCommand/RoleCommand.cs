using Core.Models;
using Dapper;
using Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Cqrs.Commands.AuthCommand
{
    public interface IRoleCommand
    {
        Task<Guid> Add(Role role);
        Task Update(Role role);
        Task Delete(string id);
    }

    public class RoleCommand : IRoleCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string addSql = $@"Insert Into Roles (Name,DeleteStatus) Output Inserted.Id Values(@{nameof(Role.Name)},0)";
        private string deleteSql = $@"Update Roles Set DeleteStatus = 1 Where Id = @id";
        private string updateSql = $@"Update Roles Set Name = @{nameof(Role.Name)} Where Id = @{nameof(Role.Id)}";

        public async Task<Guid> Add(Role role)
        {
            try
            {
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<Guid>(addSql, role, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task Update(Role role)
        {
            try
            {
                await _unitOfWork.GetConnection().QueryAsync(updateSql, role, _unitOfWork.GetTransaction());
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
    }
}
