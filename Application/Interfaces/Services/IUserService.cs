using Application.Interfaces.Repositories;
using Application.Services;
using Domain.Entities;
using Domain.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync(bool? onlyActiveRecords=true);
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<IEnumerable<User>> GetUsersByConditionsAsync(Expression<Func<User, bool>> predicate);
        Task<IEnumerable<UserPermission>> GetUserPermissionsAsync(string username, string moduleCode);
    }
}
