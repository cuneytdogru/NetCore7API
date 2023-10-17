using AutoMapper;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Exceptions;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Repositories;
using NetCore7API.Domain.Extensions;
using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.Services;
using NetCore7API.Domain.DTOs.Comment;

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

            foreach (var item in response)
            {
                item.IsLikedByCurrentUser = item.Likes.Any(x => x.UserId == _tokenService.UserId);
            }

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

            await _postRepository.LoadLike(post, user.Id);

            if (!dto.IsLiked)
            {
                post.RemoveLike(user.Id);
            }
            else
            {
                post.AddLike(user.Id);
            }

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

        public async Task<PagedResponse<CommentDto, CommentFilter>> ListCommentsAsync(Guid id, CommentFilter filter)
        {
            var response = await _postRepository.ListCommentsAsync(id, filter);

            var totalCount = await _postRepository.TotalCommentCountAsync(id, filter);

            var mappedResponse = _mapper.Map<IEnumerable<CommentDto>>(response);

            return new PagedResponse<CommentDto, CommentFilter>(mappedResponse, filter, totalCount);
        }

        public async Task<CommentDto> GetCommentAsync(Guid id, Guid commentId)
        {
            var comment = await _postRepository.GetCommentAsync(commentId);

            if (comment is null || comment.PostId != id)
                throw new UserException("Comment not found.");

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<CommentDto> AddCommentAsync(Guid id, CreateCommentDto dto)
        {
            var user = await _userRepository.FindAsync(_tokenService.UserId);

            if (user is null)
                throw new UserException("Failed to find User!");

            var post = await _postRepository.FindAsync(id);

            if (post is null)
                throw new UserException("Post not found.");

            var comment = post.AddComment(user.Id, dto);

            _postRepository.Update(post);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<CommentDto> UpdateCommentAsync(Guid id, Guid commentId, UpdateCommentDto dto)
        {
            var user = await _userRepository.FindAsync(_tokenService.UserId);

            if (user is null)
                throw new UserException("Failed to find User!");

            var post = await _postRepository.FindAsync(id);

            if (post is null)
                throw new UserException("Post not found.");

            await _postRepository.LoadComment(post, commentId);

            var comment = post.UpdateComment(user.Id, commentId, dto);

            _postRepository.Update(post);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<CommentDto> RemoveCommentAsync(Guid id, Guid commentId)
        {
            var user = await _userRepository.FindAsync(_tokenService.UserId);

            if (user is null)
                throw new UserException("Failed to find User!");

            var post = await _postRepository.FindAsync(id);

            if (post is null)
                throw new UserException("Post not found.");

            await _postRepository.LoadComment(post, commentId);

            var comment = post.RemoveComment(user.Id, commentId);

            _postRepository.Update(post);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CommentDto>(comment);
        }
    }
}