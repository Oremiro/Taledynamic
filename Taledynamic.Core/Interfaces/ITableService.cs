using System.Threading.Tasks;
using Taledynamic.DAL.Models.Requests.TableRequests;
using Taledynamic.DAL.Models.Responses.TableResponses;

namespace Taledynamic.Core.Interfaces
{
    public interface ITableService
    {
        public Task<CreateTableResponse> CreateTableAsync(CreateTableRequest request);
        public Task<GetTablesByWorkspaceResponse> GetTablesByWorkspaceAsync(GetTablesByWorkspaceRequest request);
        public Task<GetTableResponse> GetTableAsync(GetTableRequest request);

        public Task<DeleteTableResponse> DeleteTableAsync(DeleteTableRequest request);
        public Task<UpdateTableResponse> UpdateTableAsync(UpdateTableRequest request);
    }
}