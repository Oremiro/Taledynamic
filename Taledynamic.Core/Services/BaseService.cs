using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Entities;

namespace Taledynamic.Core.Services
{
    public abstract class BaseService<TEntity> where TEntity : BaseEntity, new()
    {
        private DbContext _context { get; set; }

        protected BaseService(DbContext context)
        {
            _context = context;
        }

        protected virtual async Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{typeof(TEntity)} entity is null.");
            } 
            
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        

        }
        
        protected virtual async Task CreateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{typeof(TEntity)} entity is null.");
            }
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

        }
        
        protected virtual async Task DeleteAsync(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException($"[{MethodBase.GetCurrentMethod()?.Name}] {typeof(TEntity)} id is null.");
            }

            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                throw new ArgumentNullException($"[{MethodBase.GetCurrentMethod()?.Name}] {typeof(TEntity)} entity is null.");
            }
            entity.IsActive = false; 
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();

        }
        
        protected virtual async Task<TEntity> GetByIdAsync(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException($"[{MethodBase.GetCurrentMethod()?.Name}] {typeof(TEntity)} id is null.");
            }
            var entity = await _context
                .Set<TEntity>()
                .FirstOrDefaultAsync(e => e.Id == id);

            return entity;
        }
        
        protected virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var entities = await _context.Set<TEntity>().AsNoTracking().ToListAsync();
            return entities;
        }
    }
}