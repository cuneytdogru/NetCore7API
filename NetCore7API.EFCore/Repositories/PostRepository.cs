using Microsoft.EntityFrameworkCore;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Extensions;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Models.Interfaces;
using NetCore7API.Domain.Repositories;
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
        public PostRepository(BlogContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Post>> ListBlogFeedAsync(PostFilter filter)
        {
            return await Context.Set<Post>()
                .ApplyFilter(filter)
                .Include(x => x.Comments.OrderByDescending(x => x.CreatedDate).Take(1))
                .Select(x => LoadCalculatedFields(x, x.Comments.Count))
                .ToListAsync();
        }

        public async Task<Post?> GetPostDetailAsync(Guid id)
        {
            return await Context.Set<Post>()
                .Where(x => x.Id == id)
                .Include(x => x.Comments.OrderByDescending(x => x.CreatedDate).Take(5))
                .Select(x => LoadCalculatedFields(x, x.Comments.Count))
                .FirstOrDefaultAsync();
        }

        private static Post LoadCalculatedFields(Post post, int totalComments)
        {
            post.TotalComments = totalComments;

            return post;
        }
    }
}