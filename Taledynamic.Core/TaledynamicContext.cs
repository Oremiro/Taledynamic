using Microsoft.EntityFrameworkCore;
using Taledynamic.Core.Entities;

namespace Taledynamic.Core
{
    public class TaledynamicContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public TaledynamicContext(DbContextOptions<TaledynamicContext> options) : base(options)
        {
        }
    }
}