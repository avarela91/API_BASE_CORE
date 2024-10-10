using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Entities.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public async Task<IEnumerable<User>> GetUsersByConditionsAsync(Expression<Func<User, bool>> predicate)
        {
            return await _userRepository.GetByConditionAsync(predicate);
        }
        public async Task<IEnumerable<UserPermission>> GetUserPermissionsAsync(string username, string moduleCode)
        {
            return await _userRepository.GetUserPermissionsAsync(username, moduleCode);
        }
    }
}
