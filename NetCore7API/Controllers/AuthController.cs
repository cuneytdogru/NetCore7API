using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore7API.Domain.DTOs.Auth;
using NetCore7API.Domain.Results;
using NetCore7API.Domain.Services;

namespace NetCore7API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        /// <summary>
        /// Log in with username/password
        /// </summary>
        /// <param name="dto">Login Data</param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDto>> LoginAsync(LoginRequestDto dto)
        {
            var result = await _authService.Login(dto);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        /// <summary>
        /// Logout with token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> LogoutAsync([FromHeader(Name = "Authorization")] string token)
        {
            await _authService.Logout(token);

            return NoContent();
        }
    }
}