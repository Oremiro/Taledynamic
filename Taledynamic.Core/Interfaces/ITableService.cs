using System.Threading.Tasks;
using Taledynamic.Core.Entities;
using Taledynamic.Core.Models.Requests.TableRequests;
using Taledynamic.Core.Models.Requests.WorkspaceRequests;
using Taledynamic.Core.Models.Responses.TableResponses;
using Taledynamic.Core.Models.Responses.WorkspaceResponses;

namespace Taledynamic.Core.Interfaces
{
    public interface ITableService
    {
        public Task<CreateTableResponse> CreateTableAsync(CreateTableRequest request);
        public Task<GetTablesByWorkspaceResponse> GetTablesByWorkspaceAsync(GetTablesByWorkspaceRequest request);
        public Task<DeleteTableResponse> DeleteTableAsync(DeleteTableRequest request);
        public Task<UpdateTableResponse> UpdateTableAsync(UpdateTableRequest request);
    }
}