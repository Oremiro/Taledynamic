using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Taledynamic.Core.Exceptions;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Internal;
using Taledynamic.DAL.Models.Requests.FileRequests;
using Taledynamic.DAL.Models.Responses;
using Taledynamic.DAL.MongoModels;

namespace Taledynamic.Core.Services
{
    public class FileService: IFileService
    {
        private IMongoCollection<FileModel> _documents { get; }
        public FileService(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _documents = database.GetCollection<FileModel>(settings.FileCollectionName);
        }

        public async Task<GenericCreateResponse<FileDto>> CreateFileAsync(CreateFileRequest request)
        {
            var fileModel = new FileModel
            {
                FileBase64Data = request.FileBase64,
                Type = request.Type
            };
            await _documents.InsertOneAsync(fileModel);
            return new GenericCreateResponse<FileDto>
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success",
                Item = new FileDto
                {
                    Id = fileModel.Id,
                    Base64String = fileModel.FileBase64Data,
                    Type = fileModel.Type
                }
            };
        }

        public async Task<FileContentResult> GetFileAsync(GetFileRequest request)
        {

            var result =
                await _documents.FindAsync(document => document.Id == request.UId);
            FileModel fileModel = await result.FirstOrDefaultAsync();
            
            var base64data = fileModel.FileBase64Data.Split(',')[1];
            var fileType = fileModel.Type;
            byte[] bytes = Convert.FromBase64String(base64data);
            var response = new FileContentResult(bytes, fileType);
            return response;
        }

        public async Task<GenericGetResponse<FileDto>> GetFileLinkAsync(GetFileLinkRequest request)
        {
            var result =
                await _documents.FindAsync(document => document.Id == request.UId);
            FileModel fileModel = await result.FirstOrDefaultAsync();
            return new GenericGetResponse<FileDto>
            {
                StatusCode = (HttpStatusCode) 200,
                Message = "Success",
                Item = new FileDto
                {
                    Id = fileModel.Id,
                    Base64String = fileModel.FileBase64Data,
                    Type = fileModel.Type
                }
            };
        }
    }
}