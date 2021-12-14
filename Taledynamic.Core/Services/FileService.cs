using System.Threading.Tasks;
using MongoDB.Driver;
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
        private IMongoCollection<JsonModel> _documents { get; }
        public FileService(IMongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _documents = database.GetCollection<JsonModel>(settings.JsonCollectionName);
        }

        public async Task<GenericCreateResponse<FileDto>> CreateFileAsync(CreateFileRequest request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<GenericGetResponse<FileDto>> GetFileAsync(GetFileRequest request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<GenericGetResponse<FileDto>> GetFileLinkAsync(GetFileLinkRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}