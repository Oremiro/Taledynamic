using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Requests.FileRequests;
using Taledynamic.DAL.Models.Responses;

namespace Taledynamic.Api.Controllers
{
    [ApiController]
    [Route("data/[controller]")]
    public class FileController: BaseController
    { 
        private IFileService _fileService { get; }
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("get/file")]
        public async Task<GenericGetResponse<FileDto>> GetFile([FromQuery] GetFileRequest request)
        {
            Log.Information($"[{nameof(FileController)}]: Method 'GetFile' started.");
            var response = await _fileService.GetFileAsync(request);
            Log.Information($"[{nameof(FileController)}]: Method 'GetFile' ended.");
            return response;
        }

        [HttpGet("get/link")]
        public async Task<GenericGetResponse<FileDto>> GetFileLink([FromQuery] GetFileLinkRequest request)
        {
            Log.Information($"[{nameof(FileController)}]: Method 'GetFileLink' started.");
            var response = await _fileService.GetFileLinkAsync(request);
            Log.Information($"[{nameof(FileController)}]: Method 'GetFileLink' ended.");
            return response;
        }

        [HttpPost("create")]
        public async Task<GenericCreateResponse<FileDto>> CreateFile([FromBody] CreateFileRequest request)
        {
            Log.Information($"[{nameof(FileController)}]: Method 'CreateFile' started.");
            var response = await _fileService.CreateFileAsync(request);
            Log.Information($"[{nameof(FileController)}]: Method 'CreateFile' ended.");
            return response;
        }
    }
}