using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Requests.TableRequests;
using Taledynamic.DAL.Models.Responses;

namespace Taledynamic.Core.Services
{
    public partial class TableService: ITableDataService
    {
        public Task<GenericUpdateResponse<TableDataDto>> UpdateTableDataAsync(UpdateTableDataRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<GenericCreateResponse<TableDataDto>> CreateTableDataAsync(CreateTableDataRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<GenericDeleteResponse<TableDataDto>> DeleteTableDataAsync(DeleteTableDataRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<GenericGetResponse<TableDataDto>> ReadTableDataAsync(GetTableDataRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}