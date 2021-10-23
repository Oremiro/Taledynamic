using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Taledynamic.Core.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }
}