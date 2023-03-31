using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}