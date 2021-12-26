using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.TelegramRequests
{
    public class TelegramAuthorizeRequest: BaseRequest
    {
        public string TelegramUserId { get; set; }
        public string TelegramApiKey { get; set; }

        public override ValidateState IsValid()
        {
            throw new System.NotImplementedException();
        }
    }
}