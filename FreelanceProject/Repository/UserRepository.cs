using Core.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using Repository.Cqrs.Commands.AuthCommand;
using Repository.Cqrs.Queries.AuthQuery;
using Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUserRepository : IUserCommand, IUserQuery
    {

    }

    public class UserRepository : IUserRepository
    {
        private readonly IUserCommand _userCommand;
        private readonly IUserQuery _userQuery;

        public UserRepository(IUserCommand userCommand, IUserQuery userQuery)
        {
            _userCommand = userCommand;
            _userQuery = userQuery;
        }

        public async Task<Guid> Add(User user)
        {
            user.UserName = user.Lastname + "_" + 0;

            if (await _userQuery.GetByUsername(user.UserName) != null)
            {
                while (await _userQuery.GetByUsername(user.UserName) != null)
                {
                    var n = Convert.ToInt32(user.UserName.Substring(user.UserName.Length - 1)) + 1;
                    user.UserName = user.Lastname + "_" + n;
                }
            }
            user.DisplayName = user.Firstname + " " + user.Lastname;
            user.NormalizedUserName = user.UserName.ToUpper();

            var result = await _userCommand.Add(user);
            return result;
        }

        public async Task Delete(string id)
        {
            await _userCommand.Delete(id);
        }

        public async Task<bool> Exists(string id)
        {
            var result = await _userQuery.Exists(id);
            return result;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var result = await _userQuery.GetAll();
            return result;
        }

        public async Task<User> GetByEmail(string email)
        {
            var result = await _userQuery.GetByEmail(email);
            return result;
        }

        public async Task<User> GetById(string id)
        {
            var result = await _userQuery.GetById(id);
            return result;
        }

        public async Task<User> GetByUsername(string userName)
        {
            var result = await _userQuery.GetByUsername(userName);
            return result;
        }

        public async Task Update(User user)
        {
            await _userCommand.Update(user);
        }
    }
}
