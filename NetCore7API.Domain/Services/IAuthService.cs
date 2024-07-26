using NetCore7API.Domain.DTOs.Auth;
using NetCore7API.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Services
{
    public interface IAuthService
    {
        public Task<IResult<LoginResponseDto>> Login(LoginRequestDto dto);

        public Task<IResult> Logout(string token);
    }
}