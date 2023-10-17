using Microsoft.IdentityModel.Tokens;
using NetCore7API.Domain.Models.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetCore7API.Domain.Models
{
    public class Token : BaseEntity
    {
        public static readonly TimeSpan DefaultTokenExpiry = new TimeSpan(1, 0, 0);

        public Guid UserId { get; private set; }

        public string UserName { get; private set; } = string.Empty;
        public string FullName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;

        public DateTime Expires { get; private set; }

        public bool Disabled { get; private set; }

        public DateTime DisabledDate { get; private set; }

        public bool IsExpired
        { get { return DateTime.UtcNow > Expires; } }

        public string Key { get; private set; } = string.Empty;

        private void Init(Guid userId, string userName, string email, string fullName)
        {
            this.Id = new Guid();
            this.UserId = userId;
            this.UserName = userName;
            this.FullName = fullName;
            this.Email = email;
            this.Expires = DateTime.UtcNow.Add(DefaultTokenExpiry);
            this.Key = CreateToken();
        }

        private Token(Guid userId, string userName, string email, string fullName)
        {
            this.Init(userId, userName, email, fullName);
        }

        public Token(User user)
        {
            this.Init(user.Id, user.UserName, user.Email, user.FullName);
        }

        public void Disable()
        {
            this.Disabled = true;
            this.DisabledDate = DateTime.UtcNow;
        }

        private string CreateToken()
        {
            var issuer = AppSettings.Authentication.Issuer;
            var audience = AppSettings.Authentication.Audience;
            var key = Encoding.ASCII.GetBytes(AppSettings.Authentication.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(JwtRegisteredClaimNames.Sid, this.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, this.UserName),

                new Claim(JwtRegisteredClaimNames.Name, this.FullName),
                new Claim(JwtRegisteredClaimNames.Email, this.Email),
                new Claim(JwtRegisteredClaimNames.Jti, this.Id.ToString())
             }),
                Expires = this.Expires,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}