using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Taledynamic.Api.Attributes;
using Taledynamic.Core;
using Taledynamic.Core.Helpers;

namespace Taledynamic.Api.Controllers
{
    [ApiController]
    [JwtAuthorize]
    [Route("data/[controller]")]
    public class WorkspaceController: BaseController
    {
        private TaledynamicContext _context { get; set; }
        private IOptions<AppSettings> _appSettings { get; set; }
        public WorkspaceController(TaledynamicContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }

        [HttpGet("get-filtered-by-id")]
        public async Task GetFilteredByIdAsync()
        {
            throw new NotImplementedException();
        }
        
        [HttpGet("get")]
        public async Task GetByIdAsync()
        {
            throw new NotImplementedException();
        }
        
        [HttpPost("create")]
        public async Task CreateAsync()
        {
            throw new NotImplementedException();
        }
        [HttpPut("update")]
        public async Task UpdateAsync()
        {
            throw new NotImplementedException();
        }
        [HttpDelete("delete")]
        public async Task DeleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}