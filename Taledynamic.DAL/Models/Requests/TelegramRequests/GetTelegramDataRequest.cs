using System.Text;
using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.TelegramRequests
{
    public class GetTelegramDataRequest: BaseRequest
    {
        public string UId { get; set; }
        public override ValidateState IsValid()
        {
            StringBuilder sb = new StringBuilder();
            
            if (string.IsNullOrEmpty(UId))
            {
                sb.AppendLine("UId is null.");
            }

            if (sb.Length != 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, sb.ToString());
        }
    }
}