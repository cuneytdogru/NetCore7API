﻿using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.Filters;

namespace NetCore7API.Domain.Services
{
    public interface ICommentService
    {
        Task<PagedResponse<CommentDto, CommentFilter>> ListAsync(CommentFilter filter);

        Task<CommentDto> GetAsync(Guid id);

        Task<CommentDto> CreateAsync(CreateCommentDto dto);

        Task<CommentDto> UpdateAsync(Guid id, UpdateCommentDto dto);

        Task<CommentDto> HideAsync(Guid id, HideCommentDto dto);

        Task<CommentDto> DeleteAsync(Guid id);
    }
}