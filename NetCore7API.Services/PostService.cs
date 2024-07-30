using AutoMapper;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Exceptions;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Repositories;
using NetCore7API.Domain.Services;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.Results;
using NetCore7API.Domain.Errors;
using FluentValidation;

namespace NetCore7API.Services
{
    public class PostService : BaseService, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IValidator<Post> _validator;

        public PostService(
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            IPostRepository postRepository,
            IUserRepository userRepository,
            IValidator<Post> validator
            ) : base(tokenService, unitOfWork, userRepository)
        {
            _postRepository = postRepository;
            _validator = validator;
        }

        public async Task<IResult<Guid>> CreateAsync(CreatePostRequestDto dto)
        {
            var user = await GetCurrentUser();

            var post = new Post(user, dto.Text, dto.ImageURL);

            _postRepository.Add(post);

            var validationResult = await _validator.ValidateAsync(post);

            if (!validationResult.IsValid)
                return Result<Guid>.Failure(Error.ValidationErrors(validationResult.ToDictionary()));

            await _unitOfWork.SaveChangesAsync();

            return Result<Guid>.Success(post.Id);
        }

        public async Task<IResult> UpdateAsync(Guid id, UpdatePostRequestDto dto)
        {
            var user = await GetCurrentUser();

            var post = await _postRepository.FindAsync(id);

            if (post is null)
                return Result.Failure(Error.NotFound("Post not found."));

            post.Update(dto, user);

            var validationResult = await _validator.ValidateAsync(post);

            if (!validationResult.IsValid)
                return Result.Failure(Error.ValidationErrors(validationResult.ToDictionary()));

            _postRepository.Update(post);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<IResult> LikeAsync(Guid id, LikePostRequestDto dto)
        {
            var user = await GetCurrentUser();

            var post = await _postRepository.FindAsync(id);

            if (post is null)
                return Result.Failure(Error.NotFound("Post not found."));

            await _postRepository.LoadLike(post, user.Id);

            if (!dto.IsLiked)
            {
                post.RemoveLike(user);
            }
            else
            {
                post.AddLike(user);
            }

            var validationResult = await _validator.ValidateAsync(post);

            if (!validationResult.IsValid)
                return Result.Failure(Error.ValidationErrors(validationResult.ToDictionary()));

            _postRepository.Update(post);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var user = await GetCurrentUser();

            var post = await _postRepository.FindAsync(id);

            if (post is null)
                return Result.Failure(Error.NotFound("Post not found."));

            post.Delete(user);

            _postRepository.SoftDelete(post);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<IResult<Guid>> AddCommentAsync(Guid id, CreateCommentRequestDto dto)
        {
            var user = await GetCurrentUser();

            var post = await _postRepository.FindAsync(id);

            if (post is null)
                return Result<Guid>.Failure(Error.NotFound("Post not found."));

            var comment = post.AddComment(dto, user);

            _postRepository.Update(post);

            var validationResult = await _validator.ValidateAsync(post);

            if (!validationResult.IsValid)
                return Result<Guid>.Failure(Error.ValidationErrors(validationResult.ToDictionary()));

            await _unitOfWork.SaveChangesAsync();

            return Result<Guid>.Success(comment.Id);
        }

        public async Task<IResult> UpdateCommentAsync(Guid id, Guid commentId, UpdateCommentRequestDto dto)
        {
            var user = await GetCurrentUser();

            var post = await _postRepository.FindAsync(id);

            if (post is null)
                return Result.Failure(Error.NotFound("Post not found."));

            var comment = await _postRepository.LoadComment(post, commentId);

            if (comment is null)
                return Result.Failure(Error.NotFound("Comment not found."));

            post.UpdateComment(dto, comment, user);

            var validationResult = await _validator.ValidateAsync(post);

            if (!validationResult.IsValid)
                return Result.Failure(Error.ValidationErrors(validationResult.ToDictionary()));

            _postRepository.Update(post);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<IResult> RemoveCommentAsync(Guid id, Guid commentId)
        {
            var user = await GetCurrentUser();

            var post = await _postRepository.FindAsync(id);

            if (post is null)
                return Result.Failure(Error.NotFound("Post not found."));

            var comment = await _postRepository.LoadComment(post, commentId);

            if (comment is null)
                return Result.Failure(Error.NotFound("Comment not found."));

            post.RemoveComment(comment, user);

            var validationResult = await _validator.ValidateAsync(post);

            if (!validationResult.IsValid)
                return Result.Failure(Error.ValidationErrors(validationResult.ToDictionary()));

            _postRepository.Update(post);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}