using System.ComponentModel.DataAnnotations;
using System.Text;
using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.UserRequests
{
    public class TgAuthRequest:BaseRequest
    {
        [Required]
        public string TgId { get; set; }
        
        public override ValidateState IsValid()
        {
            StringBuilder sb = new StringBuilder();
            
            if (TgId == null)
            {
                sb.Append("TgUsername is not set");
            }

            if (sb.Length != 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, "Success");
        }
    }
}