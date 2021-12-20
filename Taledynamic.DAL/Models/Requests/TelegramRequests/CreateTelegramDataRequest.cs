using System.Text.Json;
using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.TelegramRequests
{
    public class CreateTelegramDataRequest: BaseRequest
    {
        public JsonElement JsonContent { get; set; }
        public override ValidateState IsValid()
        {
            throw new System.NotImplementedException();
        }
    }
}