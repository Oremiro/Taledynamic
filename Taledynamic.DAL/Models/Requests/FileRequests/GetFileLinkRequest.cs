using System.ComponentModel.DataAnnotations;
using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.FileRequests
{
    public class GetFileLinkRequest: BaseRequest
    {
        [Required]
        public string UId { get; set; }
        public override ValidateState IsValid()
        {
            throw new System.NotImplementedException();
        }
    }
}