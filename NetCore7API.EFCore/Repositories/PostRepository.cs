using Microsoft.EntityFrameworkCore;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.DTOs.User;
using NetCore7API.Domain.Extensions;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Models.Interfaces;
using NetCore7API.Domain.Repositories;
using NetCore7API.Domain.Services;
using NetCore7API.EFCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.EFCore.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly ITokenService _tokenService;

        public PostRepository(BlogContext context, ITokenService tokenService) : base(context)
        {
            _tokenService = tokenService;
        }

        public async Task<IEnumerable<Post>> ListBlogFeedAsync(PostFilter filter)
        {
            return await Context.Set<Post>()
                .ApplyFilter(filter)
                .Include(x => x.User)
                .Include(x => x.Comments.OrderByDescending(x => x.CreatedDate).Take(1))
                .ThenInclude(c => c.User)
                .Include(x => x.Likes.Where(x => x.UserId == _tokenService.UserId).Take(1))
                .ToListAsync();
        }

        public async Task<Post?> GetPostDetailAsync(Guid id)
        {
            return await Context.Set<Post>()
                .Where(x => x.Id == id)
                .Include(x => x.User)
                .Include(x => x.Comments.OrderByDescending(x => x.CreatedDate).Take(5))
                .ThenInclude(c => c.User)
                .Include(x => x.Likes.Where(x => x.UserId == _tokenService.UserId).Take(1))
                .FirstOrDefaultAsync();
        }

        public async Task<Post?> LoadLike(Post post, Guid userId)
        {
            await Context.Entry(post)
                .Collection(x => x.Likes)
                .Query()
                .Where(x => x.UserId == userId)
                .LoadAsync();

            return post;
        }

        public async Task<IEnumerable<Comment>> ListCommentsAsync(Guid id, CommentFilter filter)
        {
            return await Context.Set<Comment>()
                .Where(x => x.PostId == id)
                .Include(c => c.User)
                .OrderByDescending(x => x.CreatedDate)
                .ApplyFilter(filter).ToListAsync();
        }

        public async Task<int> TotalCommentCountAsync(Guid id, CommentFilter filter)
        {
            return await Context.Set<Comment>()
                .Where(x => x.PostId == id).
                ApplyFilter(filter, true).CountAsync();
        }

        public async Task<Comment> GetCommentAsync(Guid commentId)
        {
            return await Context.Set<Comment>().FindAsync(commentId);
        }

        public async Task<Post?> LoadComment(Post post, Guid commentId)
        {
            await Context.Entry(post)
               .Collection(x => x.Comments)
               .Query()
               .Where(x => x.Id == commentId)
               .Where(x => x.PostId == post.Id)
               .LoadAsync();

            return post;
        }
    }
}