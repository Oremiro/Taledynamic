using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Taledynamic.DAL.Entities
{
    public class User: BaseEntity
    {
        public string TgUsername { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public virtual List<RefreshToken> RefreshTokens { get; set; }

    }
}