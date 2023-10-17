using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        /// <summary>
        /// List Posts by filter including the last comment.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IEnumerable<Post>> ListBlogFeedAsync(PostFilter filter);

        Task<IEnumerable<Comment>> ListCommentsAsync(Guid id, CommentFilter filter);

        Task<Comment> GetCommentAsync(Guid commentId);

        Task<int> TotalCommentCountAsync(Guid id, CommentFilter filter);

        Task<Post?> GetPostDetailAsync(Guid id);

        Task<Post?> LoadLike(Post post, Guid userId);

        Task<Post?> LoadComment(Post post, Guid commentId);
    }
}