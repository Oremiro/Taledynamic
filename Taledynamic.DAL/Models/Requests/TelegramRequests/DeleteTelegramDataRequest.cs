using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.TelegramRequests
{
    public class DeleteTelegramDataRequest: BaseRequest
    {
        public string UId { get; set; }
        public override ValidateState IsValid()
        {
            throw new System.NotImplementedException();
        }
    }
}