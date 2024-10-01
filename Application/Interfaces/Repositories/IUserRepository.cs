using Domain.Entities;
using Domain.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository:IGenericRepository<User>
    {
        Task<IEnumerable<UserPermission>> GetUserPermissionsAsync(string username, string moduleCode);
    }

}
