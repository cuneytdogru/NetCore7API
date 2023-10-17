using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Filters;

namespace NetCore7API.Domain.Services
{
    public interface IPostService
    {
        Task<PagedResponse<PostDto, PostFilter>> ListAsync(PostFilter filter);

        Task<PostDto> GetAsync(Guid id);

        Task<PostDto> CreateAsync(CreatePostDto dto);

        Task<PostDto> UpdateAsync(Guid id, UpdatePostDto dto);

        Task<PostDto> LikeAsync(Guid id, LikePostDto dto);

        Task<PostDto> DeleteAsync(Guid id);

        Task<PagedResponse<CommentDto, CommentFilter>> ListCommentsAsync(Guid id, CommentFilter filter);

        Task<CommentDto> GetCommentAsync(Guid id, Guid commentId);

        Task<CommentDto> AddCommentAsync(Guid id, CreateCommentDto dto);

        Task<CommentDto> UpdateCommentAsync(Guid id, Guid commentId, UpdateCommentDto dto);

        Task<CommentDto> RemoveCommentAsync(Guid id, Guid commentId);
    }
}