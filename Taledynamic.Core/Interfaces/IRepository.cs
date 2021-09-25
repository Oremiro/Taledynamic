using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Taledynamic.Core.Interfaces
{
    public interface IRepository <T> where T: class
    {
        Task<T> GetAsync(int id); 
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetFilteredAsync(Expression<Func<T, bool>> filter);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entites);
        Task<IEnumerable<T>> ExecuteRawSqlAsync(string command);
    }
}   