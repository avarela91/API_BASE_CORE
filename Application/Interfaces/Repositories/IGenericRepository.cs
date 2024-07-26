using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        
        Task<IEnumerable<T>> GetAllAsync(bool? onlyActiveRecords = true);
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task<IEnumerable<TResult>> ExecuteRawSqlAsync<TResult>(string sql, params object[] parameters) where TResult : class;

    }
}
