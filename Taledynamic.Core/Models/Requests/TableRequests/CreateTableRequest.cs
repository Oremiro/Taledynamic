using System.ComponentModel.DataAnnotations;
using System.Text;
using Taledynamic.Core.Models.Internal;

namespace Taledynamic.Core.Models.Requests.TableRequests
{
    public class CreateTableRequest: BaseRequest
    {
        [Required]
        public string Name { get; set;}
        public override ValidateState IsValid()
        {
            StringBuilder sb = new StringBuilder();

            if (Name == null)
            {
                sb.AppendLine("Name for table is not set.");
            }

            if (sb.Length != 0)
            {
                return new ValidateState(false, sb.ToString());
            }

            return new ValidateState(true, "Success.");
        }
    }
}