using Microsoft.AspNetCore.Mvc;
using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models.Interfaces;
using NetCore7API.Domain.Services;
using System.Diagnostics;

namespace NetCore7API.Controllers
{
    public class PostController : BaseApiController
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<PagedResponse<PostDto, PostFilter>>> ListAsync([FromQuery] PostFilter filter)
        {
            var resource = await _postService.ListAsync(filter);

            if (resource == null)
                return NotFound();

            return Ok(resource);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<PostDto>> GetAsync(Guid id)
        {
            var resource = await _postService.GetAsync(id);
            if (resource == null)
                return NotFound();

            return Ok(resource);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<PostDto>> PostAsync([FromBody] CreatePostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resource = await _postService.CreateAsync(dto);

            if (resource == null)
                return NotFound();

            return CreatedAtAction("Get", new { id = resource.Id }, resource);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<PostDto>> PutAsync(Guid id, [FromBody] UpdatePostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resource = await _postService.UpdateAsync(id, dto);

            if (resource == null)
                return NotFound();

            return Ok(resource);
        }

        [HttpPut("{id}/like")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<PostDto>> LikeAsync(Guid id, [FromBody] LikePostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resource = await _postService.LikeAsync(id, dto);

            if (resource == null)
                return NotFound();

            return Ok(resource);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<PostDto>> DeleteAsync(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _postService.DeleteAsync(id);

            return NoContent();
        }
    }
}