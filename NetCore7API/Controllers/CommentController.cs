using Microsoft.AspNetCore.Mvc;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models.Interfaces;
using NetCore7API.Domain.Services;
using System.Diagnostics;

namespace NetCore7API.Controllers
{
    public class CommentController : BaseApiController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<CommentDto>> ListAsync([FromQuery] CommentFilter filter)
        {
            var resource = await _commentService.ListAsync(filter);

            if (resource == null)
                return NotFound();

            return Ok(resource);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<CommentDto>> GetAsync(Guid id)
        {
            var resource = await _commentService.GetAsync(id);
            if (resource == null)
                return NotFound();

            return Ok(resource);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<CommentDto>> PostAsync([FromBody] CreateCommentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resource = await _commentService.CreateAsync(dto);

            if (resource == null)
                return NotFound();

            return CreatedAtAction("Get", new { id = resource.Id }, resource);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<CommentDto>> PutAsync(Guid id, [FromBody] UpdateCommentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resource = await _commentService.UpdateAsync(id, dto);

            if (resource == null)
                return NotFound();

            return Ok(resource);
        }

        [HttpPut("{id}/hide")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<CommentDto>> HideAsync(Guid id, [FromBody] HideCommentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resource = await _commentService.HideAsync(id, dto);

            if (resource == null)
                return NotFound();

            return Ok(resource);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<CommentDto>> DeleteAsync(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _commentService.DeleteAsync(id);

            return NoContent();
        }
    }
}