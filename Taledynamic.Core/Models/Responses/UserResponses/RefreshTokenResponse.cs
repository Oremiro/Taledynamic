using Taledynamic.Core.Entities;

namespace Taledynamic.Core.Models.Responses.UserResponses
{
    public class RefreshTokenResponse: AuthenticateResponse
    {
        public RefreshTokenResponse(User user, string jwtToken, string refreshToken) : base(user, jwtToken, refreshToken)
        {
        }
    }
}