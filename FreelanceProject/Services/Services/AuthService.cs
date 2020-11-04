using Api.Infrastructure.Helpers;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IAuthService
    {
        Task<Guid> AddRole(Role role);
        Task UpdateRole(Role role);
        Task DeleteRole(string id);
        Task<Role> GetRoleById(string id);
        Task<IEnumerable<Role>> GetAllRoles();

        Task<Guid> AddUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(string id);
        Task<bool> Exists(string id);
        Task<User> GetUserById(string id);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByUsername(string userName);
        Task<IEnumerable<User>> GetAllUsers();

        Task<Guid> AddUserRole(UserRole userRole);
        Task UpdateUserRole(UserRole userRole);
        Task DeleteUserRole(string id);
        Task<IEnumerable<User>> GetUsersByRoleName(string roleName);
        Task<IEnumerable<Role>> GetRolesByUserId(string userId);
        Task<bool> IsInRole(string userId, string roleName);

        Task<User> Login(string email, string password);
        Task<User> Register(User user, string password);
    }
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public AuthService(IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<User> Login(string email, string password)
        {
            var currUser = await _userRepository.GetByEmail(email);

            if (currUser == null)
            {
                return null;
            }

            if (!PasswordChecker.VerifyPasswordHash(password, currUser.PasswordHash, currUser.PasswordSalt))
            {
                return null;
            }

            return currUser;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            PasswordChecker.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var id = await _userRepository.Add(user);
            var endUser = await _userRepository.GetById(id.ToString());
            return endUser;
        }

        public async Task<Guid> AddRole(Role role)
        {
            var result = await _roleRepository.Add(role);
            return result;
        }

        public async Task<Guid> AddUser(User user)
        {
            var result = await _userRepository.Add(user);
            return result;
        }

        public async Task<Guid> AddUserRole(UserRole userRole)
        {
            var result = await _userRoleRepository.Add(userRole);
            return result;
        }

        public async Task DeleteRole(string id)
        {
            await _roleRepository.Delete(id);
        }

        public async Task DeleteUser(string id)
        {
            await _userRepository.Delete(id);
        }

        public async Task DeleteUserRole(string id)
        {
            await _userRoleRepository.Delete(id);
        }

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            var result = await _roleRepository.GetAll();
            return result;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var result = await _userRepository.GetAll();
            return result;
        }

        public async Task<Role> GetRoleById(string id)
        {
            var result = await _roleRepository.GetById(id);
            return result;
        }

        public async Task<IEnumerable<Role>> GetRolesByUserId(string userId)
        {
            var result = await _userRoleRepository.GetRolesByUserId(userId);
            return result;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var result = await _userRepository.GetByEmail(email);
            return result;
        }

        public async Task<User> GetUserById(string id)
        {
            var result = await _userRepository.GetById(id);
            return result;
        }

        public async Task<User> GetUserByUsername(string userName)
        {
            var result = await _userRepository.GetByUsername(userName);
            return result;
        }

        public async Task<IEnumerable<User>> GetUsersByRoleName(string roleName)
        {
            var result = await _userRoleRepository.GetUsersByRoleName(roleName);
            return result;
        }

        public async Task<bool> IsInRole(string userId, string roleName)
        {
            var result = await _userRoleRepository.IsInRole(userId, roleName);
            return result;
        }

        public async Task UpdateRole(Role role)
        {
            await _roleRepository.Update(role);
        }

        public async Task UpdateUser(User user)
        {
            await _userRepository.Update(user);
        }

        public async Task UpdateUserRole(UserRole userRole)
        {
            await _userRoleRepository.Update(userRole);
        }

        public async Task<bool> Exists(string id)
        {
            var result = await _userRepository.Exists(id);
            return result;
        }
    }
}
