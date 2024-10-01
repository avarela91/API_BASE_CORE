using Application.Interfaces.Services;
using Dapper;
using Domain.Entities.CustomAttributes;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DapperService : IDapperService
    {
        private readonly IConfiguration _configuration;

        public DapperService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        private string GetConnectionString<T>()
        {
            var connectionNameAttribute = typeof(T).GetCustomAttribute<ConnectionNameAttribute>();
            if (connectionNameAttribute != null)
            {
                return _configuration.GetConnectionString(connectionNameAttribute.ConnectionName);
            }
            else
            {
                throw new InvalidOperationException($"The entity {typeof(T).Name} does not have a ConnectionNameAttribute.");
            }
        }

        public async Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object parameters = null)
        {
            var connectionString = GetConnectionString<TResult>();
            using (var connection = CreateConnection(connectionString))
            {
                return await connection.QueryAsync<TResult>(sql, parameters);
            }
        }

        public async Task<int> ExecuteAsync(string sql, object parameters = null)
        {
            var connectionString = GetConnectionString<dynamic>();
            using (var connection = CreateConnection(connectionString))
            {
                return await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
