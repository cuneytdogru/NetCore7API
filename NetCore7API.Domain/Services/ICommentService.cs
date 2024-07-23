using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.Filters;

namespace NetCore7API.Domain.Services
{
    public interface ICommentService
    {
        Task CreateAsync(CreateCommentRequestDto dto);

        Task UpdateAsync(Guid id, UpdateCommentRequestDto dto);

        Task HideAsync(Guid id, HideCommentRequestDto dto);

        Task DeleteAsync(Guid id);
    }
}