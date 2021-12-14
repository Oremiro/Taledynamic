using System.Threading.Tasks;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Requests.FileRequests;
using Taledynamic.DAL.Models.Requests.TableRequests;
using Taledynamic.DAL.Models.Responses;
using Taledynamic.DAL.Models.Responses.TableResponses;

namespace Taledynamic.Core.Interfaces
{
    public interface IFileService
    {
        public Task<GenericCreateResponse<FileDto>> CreateFileAsync(CreateFileRequest request);
        public Task<GenericGetResponse<FileDto>> GetFileAsync(GetFileRequest request);
        public Task<GenericGetResponse<FileDto>> GetFileLinkAsync(GetFileLinkRequest request);
    }
}