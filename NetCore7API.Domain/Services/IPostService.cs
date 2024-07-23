using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Filters;

namespace NetCore7API.Domain.Services
{
    public interface IPostService
    {
        Task<Guid> CreateAsync(CreatePostRequestDto dto);

        Task UpdateAsync(Guid id, UpdatePostRequestDto dto);

        Task LikeAsync(Guid id, LikePostRequestDto dto);

        Task DeleteAsync(Guid id);

        Task<Guid> AddCommentAsync(Guid id, CreateCommentRequestDto dto);

        Task UpdateCommentAsync(Guid id, Guid commentId, UpdateCommentRequestDto dto);

        Task RemoveCommentAsync(Guid id, Guid commentId);
    }
}