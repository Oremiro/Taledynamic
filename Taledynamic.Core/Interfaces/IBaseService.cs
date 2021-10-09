using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Taledynamic.Core.Entities;

namespace Taledynamic.Core.Interfaces
{
    public interface IBaseService<TEntity> where TEntity : BaseEntity, new()
    {
        public Task CreateAsync(TEntity entity);

        public Task DeleteAsync(int? id);

        public Task<TEntity> GetByIdAsync(int? id);

        public Task<IEnumerable<TEntity>> GetAllAsync();
    }
}