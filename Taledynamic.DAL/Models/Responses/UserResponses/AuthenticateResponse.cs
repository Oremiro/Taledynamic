using System.Text.Json.Serialization;

namespace Taledynamic.DAL.Models.Responses.UserResponses
{
    public class AuthenticateResponse: BaseResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] public string RefreshToken { get; set; }
    }
}