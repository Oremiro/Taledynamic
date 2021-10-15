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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}