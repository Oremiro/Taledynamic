using System.Text.Json;
using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.TelegramRequests
{
    public class UpdateTelegramDataRequest: BaseRequest
    {
        public string UId { get; set; }
        public JsonElement JsonContent { get; set; }
        public override ValidateState IsValid()
        {
            throw new System.NotImplementedException();
        }
    }
}