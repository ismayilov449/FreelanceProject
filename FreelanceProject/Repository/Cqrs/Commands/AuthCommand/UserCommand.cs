using Core.Models;
using Dapper;
using Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Cqrs.Commands.AuthCommand
{
    public interface IUserCommand
    {
        Task<Guid> Add(User user);
        Task Update(User user);
        Task Delete(string id);
    }

    public class UserCommand : IUserCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string addSql = @$"Insert Into Users (UserName,
NormalizedUserName,
Email,
PhoneNumber,
DeleteStatus,
Firstname,
Lastname,
PasswordHash,
PasswordSalt,
DisplayName,
Address,
CreatedDate)
Output Inserted.Id
Values (@{nameof(User.UserName)},
@{nameof(User.NormalizedUserName)},
@{nameof(User.Email)},
@{nameof(User.PhoneNumber)},
0,
@{nameof(User.Firstname)},
@{nameof(User.Lastname)},
@{nameof(User.PasswordHash)},
@{nameof(User.PasswordSalt)},
@{nameof(User.DisplayName)},
@{nameof(User.Address)},
GETDATE())
";

        private string deleteSql = $@"Update Users Set DeleteStatus = 1 Where Id = @id";

        private string updateSql = $@"Update Users Set 
UserName = @{nameof(User.UserName)},
NormalizedUserName = @{nameof(User.NormalizedUserName)},
Email = @{nameof(User.Email)},
PhoneNumber = @{nameof(User.PhoneNumber)},
Firstname = @{nameof(User.Firstname)},
Lastname = @{nameof(User.Lastname)},
PasswordHash = @{nameof(User.PasswordHash)},
PasswordSalt = @{nameof(User.PasswordSalt)},
DisplayName = @{nameof(User.DisplayName)},
Address = @{nameof(User.Address)},
CreatedDate = @{nameof(User.CreatedDate)}

Where Id  = @{nameof(User)}
";

        public async Task<Guid> Add(User user)
        {
            try
            {
               
                var result = await _unitOfWork.GetConnection().QueryFirstOrDefaultAsync<Guid>(addSql, user, _unitOfWork.GetTransaction());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Update(User user)
        {
            try
            {
                await _unitOfWork.GetConnection().QueryAsync(updateSql, user, _unitOfWork.GetTransaction());
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
