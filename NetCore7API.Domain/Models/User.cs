using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.DTOs.User;
using NetCore7API.Domain.Exceptions;
using NetCore7API.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Models
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }

        public User(string userName, string email, string password, string fullName)
        {
            this.UserName = userName;
            this.Email = email;
            this.Password = password;
            this.FullName = fullName;
        }

        public void Update(UpdateUserDto dto)
        {
            this.UserName = dto.UserName;
            this.FullName = dto.FullName;
        }

        public void ChangePassword(ChangePasswordDto dto)
        {
            if (this.Password != dto.OldPassword)
                throw new UserException("Old password is invalid!");

            this.Password = dto.NewPassword;
        }
    }
}