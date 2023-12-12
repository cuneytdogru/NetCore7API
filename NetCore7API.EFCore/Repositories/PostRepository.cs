using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NetCore7API.Domain.DTOs;
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
        private readonly IMapper _mapper;

        public PostRepository(BlogContext context, ITokenService tokenService, IMapper mapper) : base(context)
        {
            _tokenService = tokenService;
            _mapper = mapper;
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