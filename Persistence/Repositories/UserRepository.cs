using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Entities.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UserRepository:GenericRepository<User>,IUserRepository
    {
        private readonly IDapperService _dapperService;
        public UserRepository(IConfiguration configuration, IDapperService dapperService) : base(configuration)
        {
            _dapperService = dapperService;
        }
        // Método asíncrono para obtener los permisos del usuario
        public async Task<IEnumerable<UserPermission>> GetUserPermissionsAsync(string userName, string codeModule)
        {
            var sql = "EXEC PermissionByUserAndModule @UserName, @CodeModule";
            var parameters = new { UserName = userName, CodeModule = codeModule };

            return await _dapperService.QueryAsync<UserPermission>(sql, parameters);
        }
    }
}
