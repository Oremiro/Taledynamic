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
using Taledynamic.DAL.Entities;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Internal;
using Taledynamic.DAL.Models.Requests.TableRequests;
using Taledynamic.DAL.Models.Requests.TelegramRequests;
using Taledynamic.DAL.Models.Responses;
using Taledynamic.DAL.MongoModels;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Taledynamic.Core.Services
{
    public partial class GSheetsService : IGSheetsDataService
    {
        public async Task<EmptyUpdateResponse> ExportTableDataAsync(UpdateTelegramDataRequest request, User user)
        {
            string json = JsonSerializer.Serialize(request.JsonContent);
            var jsonModel = new TelegramJson()
            {
                Id = request.UId,
                PostgreSQLId = user.Id,
                Document = BsonDocument.Parse(json)
            };
            
            ReplaceOneResult result = await _documents.ReplaceOneAsync(document => document.Id == request.UId, jsonModel);
            return new EmptyUpdateResponse
            {
                StatusCode = (HttpStatusCode) 200,
                Message = null,
            };
        }
        ///
        public async Task<GenericCreateResponse<TelegramDataDto>> CreateTelegramDataAsync(CreateTelegramDataRequest request, User user)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(request.JsonContent);
            var jsonModel = new TelegramJson()
            {
                PostgreSQLId = user.Id,
                Document = BsonDocument.Parse(json)
            };
            
            await _documents.InsertOneAsync(jsonModel);
            
            
            var dotnetValue = BsonTypeMapper.MapToDotNetValue(jsonModel.Document);
            var validJsonString = JsonConvert.SerializeObject(dotnetValue);

            var response = new GenericCreateResponse<TelegramDataDto>
            {
                StatusCode = (HttpStatusCode) 200,
                Message = null,
                Item = new TelegramDataDto
                {
                    UId = jsonModel.Id,
                    TableData = validJsonString
                }
            };
            
            return response;
        }

        public async Task<GenericDeleteResponse<TelegramDataDto>> DeleteTelegramDataAsync(DeleteTelegramDataRequest request, User user)
        {
            var result = await _documents.DeleteOneAsync(document => document.Id == request.UId);
            return null;
        }

        public async Task<GenericGetResponse<TelegramDataDto>> ReadTelegramDataAsync(GetTelegramDataRequest request, User user)
        {
            var table = await _context
                .Tables
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.IsActive);

            if (table == null)
            {
                throw new NotFoundException("Table with requested ids is not found");
            }
            
            var result =
                await _documents.FindAsync(document => document.Id == request.UId);
            
            TelegramJson jsonModel = result.FirstOrDefault();

            var dotnetValue = BsonTypeMapper.MapToDotNetValue(jsonModel.Document);
            var validJsonString = JsonConvert.SerializeObject(dotnetValue);
            
            var response = new GenericGetResponse<TelegramDataDto>
            {
                StatusCode = (HttpStatusCode) 200,
                Message = null,
                Item = new TelegramDataDto()
                {
                    UId = request.UId,
                    TableData = validJsonString
                }
            };
            
            return response;
        }
    }
}