using System.ComponentModel.DataAnnotations;
using System.Text;
using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.UserRequests
{
    public class GetUserRequest: BaseRequest
    {
        [Required]
        public int Id { get; set; }

        public override ValidateState IsValid()
        {
            StringBuilder sb = new StringBuilder();

            if (Id == default)
            {
                sb.Append("Id is default.");
            }

            if (sb.Length != 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, "Success");
        }
    }
}