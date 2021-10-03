using System.Text.Json.Serialization;
using Taledynamic.Core.Entities;

namespace Taledynamic.Core.Models.Responses
{
    public class AuthenticateResponse: BaseResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }

        public AuthenticateResponse(User user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            Email = user.Email;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}