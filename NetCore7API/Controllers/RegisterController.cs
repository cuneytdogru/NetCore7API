using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCore7API.Domain.DTOs.User;
using NetCore7API.Domain.Providers;
using NetCore7API.Domain.Services;

namespace NetCore7API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IUserProvider _userProvider;
        private readonly ITokenService _tokenService;

        public RegisterController(IUserService userService, IUserProvider userProvider, ITokenService tokenService)
        {
            _userService = userService;
            _userProvider = userProvider;
            _tokenService = tokenService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult> RegisterAsync([FromBody] RegisterUserRequestDto dto)
        {
            var result = await _userService.RegisterAsync(dto);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return CreatedAtAction("Get", new { id = result.Value });
        }

        [HttpGet("check-username")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<bool>> CheckUsernameAsync(string username)
        {
            var result = await _userProvider.IsUserNameInUse(username);

            return Ok(result);
        }

        [HttpGet("check-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<bool>> CheckEmailAsync(string email)
        {
            var result = await _userProvider.IsEmailInUse(email);

            return Ok(result);
        }
    }
}