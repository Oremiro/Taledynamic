using System.Text.Json;
using Taledynamic.DAL.Models.Internal;

namespace Taledynamic.DAL.Models.Requests.TableRequests
{
    public class CreateTableDataRequest: BaseRequest
    { 
        public JsonElement JsonContent { get; set; }
        public override ValidateState IsValid()
        {
            throw new System.NotImplementedException();
        }
    }
}