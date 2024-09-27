using Application.Interfaces.Repositories;
using Domain.Entities.CustomAttributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<T>: IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionStringName;
        public GenericRepository(IConfiguration configuration)
        {
            _connectionStringName = GetConnectionName();

            var connectionString = configuration.GetConnectionString(_connectionStringName);
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            _context = new ApplicationDbContext(options);
        }
        private string GetConnectionName()
        {
            var connectionNameAttribute = typeof(T).GetCustomAttribute<ConnectionNameAttribute>();
            if (connectionNameAttribute != null)
            {
                return connectionNameAttribute.ConnectionName;
            }
            else
            {
                throw new InvalidOperationException($"The entity {typeof(T).Name} does not have a ConnectionNameAttribute.");
            }
        }
        public async Task<IEnumerable<T>> GetAllAsync(bool? onlyActiveRecords = true)
        {
            if (onlyActiveRecords.HasValue && onlyActiveRecords.Value)
                return await _context.Set<T>().Where(e => EF.Property<bool>(e, "Active") == onlyActiveRecords).ToListAsync();
            else
                return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Método para ejecutar consultas SQL y devolver resultados
        /*public async Task<List<TResult>> ExecuteStoredProcedureAsync<TResult>(string storedProcedure, params object[] parameters) where TResult : class
        {
            var sqlQuery = $"{storedProcedure} {string.Join(", ", parameters)}";
            return await _context.Set<TResult>().FromSqlRaw(sqlQuery).ToListAsync();
            return await _context.Set<TResult>().FromSqlRaw(sqlQuery).AsNoTracking().ToListAsync();
        }*/
        public async Task<List<TResult>> ExecuteStoredProcedureAsync<TResult>(string storedProcedure, params object[] parameters) where TResult : class
        {
            // Formato correcto para la consulta con parámetros
            /*return await _context.Set<TResult>()
                .FromSqlRaw(storedProcedure, parameters) // Parámetros seguros
                .AsNoTracking() // Para consultas de solo lectura
                .ToListAsync();*/
            return await _context.Database
       .SqlQueryRaw<TResult>(storedProcedure, parameters)
       .ToListAsync();
        }
        // Método para ejecutar consultas SQL que no devuelvan resultados (INSERT, UPDATE, DELETE)
        public int ExecuteNonQuery(string sqlQuery, params object[] parameters)
        {
            return _context.Database.ExecuteSqlRaw(sqlQuery, parameters);
        }
    }
}
