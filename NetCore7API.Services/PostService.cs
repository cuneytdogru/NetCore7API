using AutoMapper;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Exceptions;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Repositories;
using NetCore7API.Domain.Extensions;
using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.Services;

namespace NetCore7API.Services
{
    public class PostService : Domain.Services.IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostService(
            IPostRepository postRepository,
            IUserRepository userRepository,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<PostDto, PostFilter>> ListAsync(PostFilter filter)
        {
            var response = await _postRepository.ListBlogFeedAsync(filter);

            var totalCount = await _postRepository.TotalCountAsync(filter);

            var mappedResponse = _mapper.Map<IEnumerable<PostDto>>(response);

            return new PagedResponse<PostDto, PostFilter>(mappedResponse, filter, totalCount);
        }

        public async Task<PostDto> GetAsync(Guid id)
        {
            var post = await _postRepository.GetPostDetailAsync(id);

            if (post is null)
                throw new UserException("Post not found.");

            return _mapper.Map<PostDto>(post);
        }

        public async Task<PostDto> CreateAsync(CreatePostDto dto)
        {
            var user = await _userRepository.FindAsync(_tokenService.UserId);

            if (user is null)
                throw new UserException("Failed to find User!");

            var post = new Post(user.Id, dto.Text, dto.ImageURL);

            _postRepository.Add(post);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PostDto>(post);
        }

        public async Task<PostDto> UpdateAsync(Guid id, UpdatePostDto dto)
        {
            var post = await _postRepository.FindAsync(id);

            if (post is null)
                throw new UserException("Post not found.");

            if (post.UserId != _tokenService.UserId)
                throw new UserException("You are not authorized to modify this post.");

            post.Update(dto);

            _postRepository.Update(post);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PostDto>(post);
        }

        public async Task<PostDto> LikeAsync(Guid id, LikePostDto dto)
        {
            var user = await _userRepository.FindAsync(_tokenService.UserId);

            if (user is null)
                throw new UserException("Failed to find User!");

            var post = await _postRepository.FindAsync(id);

            if (post is null)
                throw new UserException("Post not found.");

            if (!dto.IsLiked)
                await _postRepository.LoadLike(post, user.Id);

            post.Like(user.Id, dto);

            _postRepository.Update(post);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PostDto>(post);
        }

        public async Task<PostDto> DeleteAsync(Guid id)
        {
            var post = await _postRepository.FindAsync(id);

            if (post is null)
                throw new Exception("Post not found.");

            if (post.UserId != _tokenService.UserId)
                throw new UserException("You are not authorized to modify this post.");

            _postRepository.SoftDelete(post);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PostDto>(post);
        }
    }
}