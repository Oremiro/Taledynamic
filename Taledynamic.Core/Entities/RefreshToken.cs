using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Taledynamic.Core.Entities
{
    [Owned]
    public class RefreshToken: BaseEntity
    {
        [Key]
        [JsonIgnore]
        public new int Id { get; set; }
        
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public new bool IsActive => Revoked == null && !IsExpired;
    }
}