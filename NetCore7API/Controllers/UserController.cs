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

        public UserController(IUserService userService, IUserProvider userProvider)
        {
            _userService = userService;
            _userProvider = userProvider;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<UserDto>> GetAsync(Guid id)
        {
            var resource = await _userProvider.GetUserAsync(id);
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

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<UserDto>> PutAsync(Guid id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resource = await _userService.UpdateAsync(id, dto);

            if (resource == null)
                return NotFound();

            return Ok(resource);
        }

        [HttpPut("{id}/change-password")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<UserDto>> ChangePasswordAsync(Guid id, [FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resource = await _userService.ChangePasswordAsync(id, dto);

            if (resource == null)
                return NotFound();

            return Ok(resource);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<UserDto>> DeleteAsync(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userService.DeleteAsync(id);

            return NoContent();
        }
    }
}