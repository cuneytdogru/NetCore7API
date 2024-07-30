using NetCore7API.Domain.DTOs;
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
        public static User SystemUser = new User("System", "system@angularblog.com", "System User");

        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string FullName { get; private set; }
        public string? Password { get; private set; } = null;
        public string? PasswordSalt { get; private set; } = null;

        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }

        public User(string userName, string email, string fullName) : this(userName, email, fullName, null)
        {
        }

        public User(string userName, string email, string fullName, string? rawPassword)
        {
            this.UserName = userName;
            this.Email = email;
            this.FullName = fullName;

            if (rawPassword is not null)
                this.SetPassword(rawPassword);

            this.Posts = new HashSet<Post>();
            this.Comments = new HashSet<Comment>();
            this.Likes = new HashSet<Like>();
        }

        public void Update(UpdateUserRequestDto dto, User user)
        {
            if (this.Id != user.Id)
                throw new UserUnauthorizedException("You are not authorized to modify this user.");

            this.UserName = dto.UserName;
            this.FullName = dto.FullName;
        }

        public void Delete(User user)
        {
            if (this.Id != user.Id)
                throw new UserUnauthorizedException("You are not authorized to modify this user.");

            this.Deleted = true;
        }

        internal void SetPassword(string password)
        {
            this.PasswordSalt = PasswordHasher.GenerateSalt();
            this.Password = this.ComputeHash(password);
        }

        public void ChangePassword(ChangePasswordRequestDto dto, User user)
        {
            if (this.Id != user.Id)
                throw new UserUnauthorizedException("You are not authorized to modify this user.");

            if (this.Password is not null && this.Password != dto.OldPassword)
                throw new UserException("Old password is invalid!");

            SetPassword(dto.NewPassword);
        }

        private string ComputeHash(string password)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(password, nameof(password));
            ArgumentNullException.ThrowIfNullOrEmpty(this.PasswordSalt, nameof(PasswordSalt));

            return PasswordHasher.ComputeHash(
                password,
                this.PasswordSalt,
                AppSettings.Password.Pepper,
                AppSettings.Password.Iteration);
        }

        public bool CheckPassword(string password)
        {
            if (this.Password == this.ComputeHash(password))
                return true;

            return false;
        }
    }
}