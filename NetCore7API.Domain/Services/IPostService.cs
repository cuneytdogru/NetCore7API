using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Results;

namespace NetCore7API.Domain.Services
{
    public interface IPostService
    {
        Task<IResult<Guid>> CreateAsync(CreatePostRequestDto dto);

        Task<IResult> UpdateAsync(Guid id, UpdatePostRequestDto dto);

        Task<IResult> LikeAsync(Guid id, LikePostRequestDto dto);

        Task<IResult> DeleteAsync(Guid id);

        Task<IResult<Guid>> AddCommentAsync(Guid id, CreateCommentRequestDto dto);

        Task<IResult> UpdateCommentAsync(Guid id, Guid commentId, UpdateCommentRequestDto dto);

        Task<IResult> RemoveCommentAsync(Guid id, Guid commentId);
    }
}