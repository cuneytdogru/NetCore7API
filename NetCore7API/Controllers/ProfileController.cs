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
    public class ProfileController : BaseApiController
    {
        private readonly IUserProvider _userProvider;

        public ProfileController(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        /// <summary>
        /// Gets profile of the user.
        /// </summary>
        /// <returns>User</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<ProfileDto>> GetProfileByUserNameAsync([FromQuery] string userName)
        {
            var resource = await _userProvider.GetProfileByUserNameAsync(userName);

            if (resource == null)
                return NotFound();

            return Ok(resource);
        }

        /// <summary>
        /// Gets posts of the user.
        /// </summary>
        /// <returns>User</returns>
        [HttpGet("posts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<ProfileDto>> GetProfilePostsAsync([FromQuery] PostFilter filter)
        {
            var resource = await _userProvider.GetProfilePostsAsync(filter);

            if (resource == null)
                return NotFound();

            return Ok(resource);
        }

        /// <summary>
        /// Gets comments of the user.
        /// </summary>
        /// <returns>User</returns>
        [HttpGet("comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<ProfileDto>> GetProfileCommentsAsync([FromQuery] CommentFilter filter)
        {
            var resource = await _userProvider.GetProfileCommentsAsync(filter);

            if (resource == null)
                return NotFound();

            return Ok(resource);
        }
    }
}