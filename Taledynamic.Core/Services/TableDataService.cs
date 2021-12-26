using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Taledynamic.Core.Exceptions;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Requests.TableRequests;
using Taledynamic.DAL.Models.Responses;
using Taledynamic.DAL.MongoModels;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Taledynamic.Core.Services
{
    public partial class TableService: ITableDataService
    {
        public async Task<EmptyUpdateResponse> UpdateTableDataAsync(UpdateTableDataRequest request)
        {
            string json = JsonSerializer.Serialize(request.JsonContent);
            var jsonModel = new JsonModel()
            {
                Id = request.UId,
                Document = BsonDocument.Parse(json)
            };
            
            ReplaceOneResult result = await _documents.ReplaceOneAsync(document => document.Id == request.UId, jsonModel);
            return new EmptyUpdateResponse
            {
                StatusCode = (HttpStatusCode) 200,
                Message = null,
            };
        }

        public async Task<GenericCreateResponse<TableDataDto>> CreateTableDataAsync(CreateTableDataRequest request, int? id)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(request.JsonContent);
            var jsonModel = new JsonModel()
            {
                Document = BsonDocument.Parse(json)
            };
            
            await _documents.InsertOneAsync(jsonModel);
            
            var table = await _context
                .Tables
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.IsActive && w.Id == id);

            if (table == null)
            {
                throw new NotFoundException("Table with requested ids is not found");
            }
            
            table.MongoDbUId = jsonModel.Id;
            await this.UpdateAsync(table);
            
            var dotnetValue = BsonTypeMapper.MapToDotNetValue(jsonModel.Document);
            var validJsonString = JsonConvert.SerializeObject(dotnetValue);

            var response = new GenericCreateResponse<TableDataDto>
            {
                StatusCode = (HttpStatusCode) 200,
                Message = null,
                Item = new TableDataDto
                {
                    UId = jsonModel.Id,
                    TableData = validJsonString
                }
            };
            
            return response;
        }

        public async Task<GenericDeleteResponse<TableDataDto>> DeleteTableDataAsync(DeleteTableDataRequest request)
        {
            var result = await _documents.DeleteOneAsync(document => document.Id == request.UId);
            return null;
        }

        public async Task<GenericGetResponse<TableDataDto>> ReadTableDataAsync(GetTableDataRequest request, int? id)
        {
            var table = await _context
                .Tables
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.IsActive && t.Id == id);

            if (table == null)
            {
                throw new NotFoundException("Table with requested ids is not found");
            }
            
            if (table.MongoDbUId == null)
            {
                var fakeDoc = JsonDocument.Parse("{}");
                var fakeDocRootElement = fakeDoc.RootElement;
                var fakeRequest = new CreateTableDataRequest
                {
                    JsonContent = fakeDocRootElement
                };
                
                var res = await CreateTableDataAsync(fakeRequest, id);
                var uId = res.Item.UId;
                return new GenericGetResponse<TableDataDto>
                {
                    StatusCode = (HttpStatusCode) 200,
                    Message = null,
                    Item = new TableDataDto()
                    {
                        UId = uId,
                        TableData = fakeDocRootElement.ToJson()
                    }
                };
            }
            var validator = request.IsValid();
            if (!validator.Status)
            {
                throw new BadRequestException(validator.Message);
            }
            
            var result =
                await _documents.FindAsync(document => document.Id == table.MongoDbUId);
            
            JsonModel jsonModel = result.FirstOrDefault();

            var dotnetValue = BsonTypeMapper.MapToDotNetValue(jsonModel.Document);
            var validJsonString = JsonConvert.SerializeObject(dotnetValue);
            
            var response = new GenericGetResponse<TableDataDto>
            {
                StatusCode = (HttpStatusCode) 200,
                Message = null,
                Item = new TableDataDto()
                {
                    UId = table.MongoDbUId,
                    TableData = validJsonString
                }
            };
            
            return response;
        }
    }
}