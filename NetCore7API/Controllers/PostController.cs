using Microsoft.AspNetCore.Mvc;
using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Models.Interfaces;
using NetCore7API.Domain.Providers;
using NetCore7API.Domain.Repositories;
using NetCore7API.Domain.Services;
using NetCore7API.EFCore.Providers;
using System.Diagnostics;

namespace NetCore7API.Controllers
{
    public class PostController : BaseApiController
    {
        private readonly IPostService _postService;
        private readonly IPostProvider _postProvider;

        public PostController(IPostService postService, IPostProvider postProvider)
        {
            _postService = postService;
            _postProvider = postProvider;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<PagedResponse<PostDto, PostFilter>>> ListAsync([FromQuery] PostFilter filter)
        {
            var blogFeed = await _postProvider.ListBlogFeedAsync(filter);

            return Ok(blogFeed);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<ActionResult<PostDto>> GetAsync(Guid id)
        {
            var post = await _postProvider.GetPostDetailAsync(id);

            if (post is null)
                return NotFound();

            return Ok(post);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult> PostAsync([FromBody] CreatePostRequestDto dto)
        {
            var result = await _postService.CreateAsync(dto);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return CreatedAtAction("Get", new { id = result.Value }, result.Value);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult> PutAsync(Guid id, [FromBody] UpdatePostRequestDto dto)
        {
            var result = await _postService.UpdateAsync(id, dto);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return NoContent();
        }

        [HttpPut("{id}/like")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult> LikeAsync(Guid id, [FromBody] LikePostRequestDto dto)
        {
            var result = await _postService.LikeAsync(id, dto);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<PostDto>> DeleteAsync(Guid id)
        {
            var result = await _postService.DeleteAsync(id);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return NoContent();
        }

        [HttpGet("{id}/comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<PagedResponse<CommentDto, CommentFilter>>> ListAsync(Guid id, [FromQuery] CommentFilter filter)
        {
            var comments = await _postProvider.ListCommentsAsync(id, filter);

            return Ok(comments);
        }

        [HttpGet("{id}/comments/{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<ActionResult<PostDto>> GetCommentAsync(Guid id, Guid commentId)
        {
            var comment = await _postProvider.GetCommentAsync(commentId);

            if (comment is null)
                return NotFound();

            return Ok(comment);
        }

        [HttpPost("{id}/comments")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult> PostCommentAsync(Guid id, [FromBody] CreateCommentRequestDto dto)
        {
            var result = await _postService.AddCommentAsync(id, dto);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return CreatedAtAction(
                "GetComment",
                new { id, commentId = result.Value },
                result.Value);
        }

        [HttpPut("{id}/comments/{commentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult> PutCommentAsync(Guid id, Guid commentId, [FromBody] UpdateCommentRequestDto dto)
        {
            var result = await _postService.UpdateCommentAsync(id, commentId, dto);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return NoContent();
        }

        [HttpDelete("{id}/comments/{commentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult> DeleteCommentAsync(Guid id, Guid commentId)
        {
            var result = await _postService.RemoveCommentAsync(id, commentId);

            if (result.IsFailure)
                return BadRequest(result.Errors);

            return NoContent();
        }
    }
}