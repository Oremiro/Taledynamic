using Microsoft.EntityFrameworkCore;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Entities;

namespace Taledynamic.Core.Services
{
    public partial class TelegramService: BaseService<TelegramUser>, ITelegramService
    {
        public TelegramService(DbContext context) : base(context)
        {
        }
        
        
    }
}