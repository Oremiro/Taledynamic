using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Taledynamic.Core.Entities
{
    public class User: BaseEntity
    {
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public virtual List<RefreshToken> RefreshTokens { get; set; }

    }
}