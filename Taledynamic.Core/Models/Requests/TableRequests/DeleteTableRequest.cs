using System.ComponentModel.DataAnnotations;
using System.Text;
using Taledynamic.Core.Models.Internal;

namespace Taledynamic.Core.Models.Requests.TableRequests
{
    public class DeleteTableRequest: BaseRequest
    {
        [Required]
        public int Id { get; set; }
        public override ValidateState IsValid()
        {
            StringBuilder sb = new StringBuilder();
            
            if (Id == default)
            {
                sb.AppendLine("Id is default.");
            }

            if (sb.Length != 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, sb.ToString());
        }
    }
}