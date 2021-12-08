using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Taledynamic.Api.Attributes;

namespace Taledynamic.Api.Controllers
{
    [ApiController]
    [JwtAuthorize]
    [Route("integration/[controller]")]
    public class TelegramController: BaseController
    {
        public TelegramController()
        {
            
        }

        [HttpPost("create")]
        public void Create()
        {
            throw new NotImplementedException();
        }

        [HttpGet("get")]
        public void Get()
        {
            throw new NotImplementedException();
        }
        [HttpPut("update")]
        public void Update()
        {
            throw new NotImplementedException();
        }
        [HttpDelete("delete")]
        public void Delete()
        {
            throw new NotImplementedException();
        }
        
        [HttpPost("data/create")]
        public void CreateData()
        {
            throw new NotImplementedException();
        }
        
        [HttpGet("data/get")]
        public void GetData()
        {
            throw new NotImplementedException();
        }
        
        [HttpPut("data/update")]
        public void UpdateData()
        {
            throw new NotImplementedException();
        }
        
        [HttpDelete("data/delete")]
        public void DeleteData()
        {
            throw new NotImplementedException();
        }
        
    }
}