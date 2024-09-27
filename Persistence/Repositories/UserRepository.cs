using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UserRepository:GenericRepository<User>,IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }
        // Método asíncrono para obtener los permisos del usuario
        public async Task<List<UserPermission>> GetUserPermissionsAsync(string userName, string codeModule)
        {
            string storedProcedure = "EXEC PermissionByUserAndModule @UserName, @CodeModule";
            return await ExecuteStoredProcedureAsync<UserPermission>(storedProcedure, userName, codeModule);
        }
    }
}
