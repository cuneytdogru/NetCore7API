using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.DTOs.User;
using NetCore7API.Domain.Exceptions;
using NetCore7API.Domain.Models.Interfaces;
using NetCore7API.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Models
{
    public class User : BaseEntity
    {
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string FullName { get; private set; }
        public string Password { get; private set; }
        public string PasswordSalt { get; private set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }

        public User(string userName, string email, string fullName, string password)
        {
            this.UserName = userName;
            this.Email = email;
            this.FullName = fullName;

            this.SetPassword(password);
        }

        public void Update(UpdateUserDto dto)
        {
            this.UserName = dto.UserName;
            this.FullName = dto.FullName;
        }

        internal void SetPassword(string password)
        {
            this.PasswordSalt = PasswordHasher.GenerateSalt();
            this.Password = PasswordHasher.ComputeHash(
                password,
                PasswordSalt,
                AppSettings.Password.Pepper,
                AppSettings.Password.Iteration);
        }

        public void ChangePassword(ChangePasswordDto dto)
        {
            if (this.Password != dto.OldPassword)
                throw new UserException("Old password is invalid!");

            SetPassword(dto.NewPassword);
        }
    }
}