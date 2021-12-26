using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taledynamic.DAL.Entities
{
    public class TelegramUser: BaseEntity
    {
        public string TgUserId { get; set; }
        public string TgApiKey { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}