using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.User;

namespace NetCore7API.Domain.Providers
{
    /// <summary>
    /// Queries for User.
    /// </summary>
    public interface IUserProvider : IProvider
    {
        Task<bool> IsUserNameInUse(string userName);

        Task<bool> IsUserNameInUse(string userName, Guid except);

        Task<bool> IsEmailInUse(string email);

        Task<bool> IsEmailInUse(string email, Guid except);

        Task<UserDto?> GetUserAsync(Guid id);

        Task<ProfileDto?> GetProfileByUserNameAsync(string userName);

        Task<PagedResponse<PostDto, PostFilter>> GetProfilePostsAsync(PostFilter filter);

        Task<PagedResponse<CommentDto, CommentFilter>> GetProfileCommentsAsync(CommentFilter filter);
    }
}