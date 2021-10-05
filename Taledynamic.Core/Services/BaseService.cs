using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taledynamic.Core.Entities;
using Taledynamic.Core.Interfaces;

namespace Taledynamic.Core.Services
{
    public class BaseService<TEntity>: IBaseService<TEntity> where TEntity : BaseEntity, new()
    {
        private DbContext _context { get; set; }

        protected BaseService(DbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{typeof(TEntity)} entity is null.");
            }
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteAsync(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException($"[{MethodBase.GetCurrentMethod()?.Name}] {typeof(TEntity)} id is null.");
            }

            var entity = await GetByIdAsync(id);
            entity.IsActive = false; 
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
        
        public async Task<TEntity> GetByIdAsync(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException($"[{MethodBase.GetCurrentMethod()?.Name}] {typeof(TEntity)} id is null.");
            }
            var entity = await _context
                .Set<TEntity>()
                .AsNoTracking()
                .FirstAsync(e => e.Id == id);
            return entity;
        }
        
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var entities = await _context.Set<TEntity>().AsNoTracking().ToListAsync();
            return entities;
        }
    }
}