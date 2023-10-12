using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.DTOs.Auth
{
    public class LoginResponseDto
    {
        public LoginResponseDto(string token)
        {
            Token = token;
        }

        public string Token { get; set; }
    }
}