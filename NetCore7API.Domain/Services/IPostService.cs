using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Filters;

namespace NetCore7API.Domain.Services
{
    public interface IPostService
    {
        Task<Guid> CreateAsync(CreatePostDto dto);

        Task UpdateAsync(Guid id, UpdatePostDto dto);

        Task LikeAsync(Guid id, LikePostDto dto);

        Task DeleteAsync(Guid id);

        Task<Guid> AddCommentAsync(Guid id, CreateCommentDto dto);

        Task UpdateCommentAsync(Guid id, Guid commentId, UpdateCommentDto dto);

        Task RemoveCommentAsync(Guid id, Guid commentId);
    }
}