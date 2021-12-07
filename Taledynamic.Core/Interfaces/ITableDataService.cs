using System.Threading.Tasks;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Requests.TableRequests;
using Taledynamic.DAL.Models.Responses;

namespace Taledynamic.Core.Interfaces
{
    public interface ITableDataService
    {
        public Task<EmptyUpdateResponse> UpdateTableDataAsync(UpdateTableDataRequest request);
        public Task<GenericCreateResponse<TableDataDto>> CreateTableDataAsync(CreateTableDataRequest request, int? id);
        public Task<GenericDeleteResponse<TableDataDto>> DeleteTableDataAsync(DeleteTableDataRequest request);
        public Task<GenericGetResponse<TableDataDto>> ReadTableDataAsync(GetTableDataRequest request, int? id);

    }
}