using NetCore7API.Domain.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Services
{
    public interface ITokenService
    {
        public JwtSecurityToken? Token { get; }

        public Guid? UserId { get; }
    }
}