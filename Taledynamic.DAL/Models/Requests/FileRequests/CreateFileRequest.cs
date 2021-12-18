using System.ComponentModel.DataAnnotations;
using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.FileRequests
{
    public class CreateFileRequest: BaseRequest
    {
        [Required]
        public string FileBase64 { get; set; }
        [Required]
        public string Type { get; set; }
        public override ValidateState IsValid()
        {
            throw new System.NotImplementedException();
        }
    }
}