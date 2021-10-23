using System.ComponentModel.DataAnnotations;

namespace Taledynamic.DAL.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }
}