using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Filters;

namespace NetCore7API.Domain.Services
{
    public interface IPostService
    {
        Task<PostDto> CreateAsync(CreatePostDto dto);

        Task<PostDto> UpdateAsync(Guid id, UpdatePostDto dto);

        Task<PostDto> LikeAsync(Guid id, LikePostDto dto);

        Task<PostDto> DeleteAsync(Guid id);

        Task<CommentDto> AddCommentAsync(Guid id, CreateCommentDto dto);

        Task<CommentDto> UpdateCommentAsync(Guid id, Guid commentId, UpdateCommentDto dto);

        Task<CommentDto> RemoveCommentAsync(Guid id, Guid commentId);
    }
}