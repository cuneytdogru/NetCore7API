using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.DTOs.User;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models.Interfaces;
using NetCore7API.Domain.Providers;
using NetCore7API.Domain.Services;
using System.Diagnostics;

namespace NetCore7API.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IUserProvider _userProvider;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, IUserProvider userProvider, ITokenService tokenService)
        {
            _userService = userService;
            _userProvider = userProvider;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Gets logged in user.
        /// </summary>
        /// <returns>User</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<UserDto>> GetAsync()
        {
            var resource = await _userProvider.GetUserAsync(_tokenService.UserId.GetValueOrDefault());

            if (resource == null)
                return NotFound();

            return Ok(resource);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<UserDto>> UserAsync([FromBody] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resource = await _userService.RegisterAsync(dto);

            if (resource == null)
                return NotFound();

            return CreatedAtAction("Get", new { id = resource.Id }, resource);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<UserDto>> PutAsync([FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resource = await _userService.UpdateAsync(_tokenService.UserId.GetValueOrDefault(), dto);

            if (resource == null)
                return NotFound();

            return Ok(resource);
        }

        [HttpPut("change-password")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<UserDto>> ChangePasswordAsync([FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resource = await _userService.ChangePasswordAsync(_tokenService.UserId.GetValueOrDefault(), dto);

            if (resource == null)
                return NotFound();

            return Ok(resource);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<UserDto>> DeleteAsync(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userService.DeleteAsync(_tokenService.UserId.GetValueOrDefault());

            return NoContent();
        }
    }
}