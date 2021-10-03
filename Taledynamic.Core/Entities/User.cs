using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Taledynamic.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }

    }
}