using NetCore7API.Domain.DTOs;
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
        Task<Post?> LoadLike(Post post, Guid userId);

        Task<Comment?> LoadComment(Post post, Guid commentId);
    }
}