using Microsoft.EntityFrameworkCore;

namespace Taledynamic.Core
{
    public class TaledynamicContext: DbContext
    {
        public TaledynamicContext(DbContextOptions<TaledynamicContext> options) : base(options)
        {
        }
    }
}