using Microsoft.EntityFrameworkCore;
using Taledynamic.Core.Entities;

namespace Taledynamic.Core
{
    public class TaledynamicContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public TaledynamicContext(DbContextOptions<TaledynamicContext> options) : base(options)
        {
        }
    }
}