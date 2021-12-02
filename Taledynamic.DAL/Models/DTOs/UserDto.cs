using System;
using Taledynamic.DAL.Entities;

namespace Taledynamic.DAL.Models.DTOs
{
    public class UserDto
    {
        public string TgUsername { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public UserDto() {}

        public UserDto(User user)
        {
            if (user == null)
            {
                throw new NullReferenceException("Table entity is empty");
            }

            Id = user.Id;
            Email = user.Email;
        }
    }
}