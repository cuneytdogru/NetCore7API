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

namespace NetCore7API.Domain.Providers
{
    /// <summary>
    /// Queries for Posts.
    /// </summary>
    public interface IPostProvider : IProvider
    {
        /// <summary>
        /// List Posts by filter including the last comment.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<PagedResponse<PostDto, PostFilter>> ListBlogFeedAsync(PostFilter filter);

        Task<PagedResponse<CommentDto, CommentFilter>> ListCommentsAsync(Guid id, CommentFilter filter);

        Task<PostDto?> GetPostDetailAsync(Guid id);

        Task<CommentDto?> GetCommentAsync(Guid commentId);
    }
}