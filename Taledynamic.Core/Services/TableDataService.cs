using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MongoDB.Bson;
using MongoDB.Driver;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Requests.TableRequests;
using Taledynamic.DAL.Models.Responses;
using Taledynamic.DAL.MongoModels;

namespace Taledynamic.Core.Services
{
    public partial class TableService: ITableDataService
    {
        public async Task<GenericUpdateResponse<TableDataDto>> UpdateTableDataAsync(UpdateTableDataRequest request)
        {
            var jsonModel = new JsonModel()
            {
                Document = BsonDocument.Parse(request.JsonContent)
            };
            
            ReplaceOneResult result = await _documents.ReplaceOneAsync(document => document.Id == request.UId, jsonModel);
            return null;
        }

        public async Task<GenericCreateResponse<TableDataDto>> CreateTableDataAsync(CreateTableDataRequest request)
        {
            var jsonModel = new JsonModel()
            {
                Document = BsonDocument.Parse(request.JsonContent)
            };
            
            await _documents.InsertOneAsync(jsonModel);
            return null;
        }

        public async Task<GenericDeleteResponse<TableDataDto>> DeleteTableDataAsync(DeleteTableDataRequest request)
        {
            var result = await _documents.DeleteOneAsync(document => document.Id == request.UId);
            return null;
        }

        public async Task<GenericGetResponse<TableDataDto>> ReadTableDataAsync(GetTableDataRequest request)
        {
            var result =
                await _documents.FindAsync(document => document.Id == request.UId);
            return null;
        }
    }
}