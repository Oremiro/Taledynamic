using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Taledynamic.Core.Interfaces;

namespace Taledynamic.Core.Repositories
{
    public class Repository<T>: IRepository<T> where T: class
    {
        public async Task<T> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetFilteredAsync(Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entites)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> ExecuteRawSqlAsync(string command)
        {
            throw new NotImplementedException();
        }
    }
}