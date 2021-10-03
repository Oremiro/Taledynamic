using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Taledynamic.Core.Entities;
using Taledynamic.Core.Interfaces;
using Taledynamic.Core.Services;
using Xunit;

namespace Core
{
    public class UserTest
    {
        private IUserService _userService { get; set; }
        public UserTest(UserService userService)
        {
            _userService = userService;
        }
        
        [Fact]
        public async Task CreateUser_Success()
        {
            // var mock = new Mock<UserService>();Mock
            // var setup = mock.Setup(service => service.GetAllAsync());
        }
        
        [Fact]
        public async Task CreateUser_Fail()
        {
            
        }
        
        [Fact]
        public async Task UpdateUser_Success()
        {
            
        }
        
        [Fact]
        public async Task UpdateUser_Fail()
        {
            
        }
        
        [Fact]
        public async Task DeleteUser_Success()
        {
            
        }
        
        [Fact]
        public async Task DeleteUser_Fail()
        {
            
        }
        
        [Fact]
        public async Task Authenticate_Success()
        {
            
        }
        
        [Fact]
        public async Task Authenticate_Fail()
        {
            
        }
    }
}