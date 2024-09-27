using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(bool? onlyActiveRecords = true)
        {
            return await _userRepository.GetAllAsync(onlyActiveRecords);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
        public async Task AddUserAsync(User user)
        {
            await _userRepository.AddAsync(user);
        }
        public async Task UpdateUserAsync(User user)
        {
           await _userRepository.UpdateAsync(user);
        }
       
        public async Task<IEnumerable<UserPermission>> GetUserPermissionsAsync(string userName, string codeModule)
        {
            string storedProcedure = "EXEC PermissionByUserAndModule @UserName, @CodeModule";
            var parameters = new object[] { new SqlParameter("@UserName", userName), new SqlParameter("@CodeModule", codeModule) };
            return await _userRepository.ExecuteStoredProcedureAsync<UserPermission>(storedProcedure, parameters);
        }
    }
}
