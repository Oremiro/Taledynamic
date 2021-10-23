using System.ComponentModel.DataAnnotations;
using System.Text;
using Taledynamic.Core.Models.Internal;

namespace Taledynamic.Core.Models.Requests.TableRequests
{
    public class GetTableRequest: BaseRequest
    {
        [Required]
        public int Id { get; set; }
        public override ValidateState IsValid()
        {
            var sb = new StringBuilder();
            if (Id == default)
            {
                sb.Append("Id contains default value");
            }

            if (sb.Length != 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, "Success");
        }
    }
}