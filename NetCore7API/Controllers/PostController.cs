using Microsoft.AspNetCore.Mvc;
using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Filters;
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
        public virtual async Task<ActionResult> PostAsync([FromBody] CreatePostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var postId = await _postService.CreateAsync(dto);

            return CreatedAtAction("Get", new { id = postId.ToString() }, postId);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult> PutAsync(Guid id, [FromBody] UpdatePostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _postService.UpdateAsync(id, dto);

            return NoContent();
        }

        [HttpPut("{id}/like")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult> LikeAsync(Guid id, [FromBody] LikePostDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _postService.LikeAsync(id, dto);

            return NoContent();
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
            var comment = await _postProvider.GetCommentAsync(id);

            if (comment is null)
                return NotFound();

            return Ok(comment);
        }

        [HttpPost("{id}/comments")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult> PostCommentAsync(Guid id, [FromBody] CreateCommentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var commentId = await _postService.AddCommentAsync(id, dto);

            return CreatedAtAction("{id}/comments/{commentId}", new { id, commentId });
        }

        [HttpPut("{id}/comments/{commentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult> PutCommentAsync(Guid id, Guid commentId, [FromBody] UpdateCommentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _postService.UpdateCommentAsync(id, commentId, dto);

            return NoContent();
        }

        [HttpDelete("{id}/comments/{commentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult> DeleteCommentAsync(Guid id, Guid commentId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _postService.RemoveCommentAsync(id, commentId);

            return NoContent();
        }
    }
}