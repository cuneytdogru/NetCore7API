using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.Filters;

namespace NetCore7API.Domain.Services
{
    public interface ICommentService
    {
        Task CreateAsync(CreateCommentDto dto);

        Task UpdateAsync(Guid id, UpdateCommentDto dto);

        Task HideAsync(Guid id, HideCommentDto dto);

        Task DeleteAsync(Guid id);
    }
}