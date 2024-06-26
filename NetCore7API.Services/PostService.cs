﻿using AutoMapper;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Exceptions;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Repositories;
using NetCore7API.Domain.Services;
using NetCore7API.Domain.DTOs.Comment;

namespace NetCore7API.Services
{
    public class PostService : IPostService
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

        public async Task<Guid> CreateAsync(CreatePostDto dto)
        {
            var user = await _userRepository.FindAsync(_tokenService.UserId.Value);

            if (user is null)
                throw new UserException("Failed to find User!");

            var post = new Post(user.Id, dto.Text, dto.ImageURL);

            _postRepository.Add(post);

            await _unitOfWork.SaveChangesAsync();

            return post.Id;
        }

        public async Task UpdateAsync(Guid id, UpdatePostDto dto)
        {
            var post = await _postRepository.FindAsync(id);

            if (post is null)
                throw new UserException("Post not found.");

            if (post.UserId != _tokenService.UserId)
                throw new UserException("You are not authorized to modify this post.");

            post.Update(dto);

            _postRepository.Update(post);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task LikeAsync(Guid id, LikePostDto dto)
        {
            if (_tokenService.UserId is null)
                throw new Exception("User is not authorized!");

            var user = await _userRepository.FindAsync(_tokenService.UserId.Value);

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
        }

        public async Task DeleteAsync(Guid id)
        {
            var post = await _postRepository.FindAsync(id);

            if (post is null)
                throw new Exception("Post not found.");

            if (post.UserId != _tokenService.UserId)
                throw new UserException("You are not authorized to modify this post.");

            _postRepository.SoftDelete(post);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Guid> AddCommentAsync(Guid id, CreateCommentDto dto)
        {
            if (_tokenService.UserId is null)
                throw new Exception("User is not authorized!");

            var user = await _userRepository.FindAsync(_tokenService.UserId.Value);

            if (user is null)
                throw new UserException("Failed to find User!");

            var post = await _postRepository.FindAsync(id);

            if (post is null)
                throw new UserException("Post not found.");

            var comment = post.AddComment(user.Id, dto);

            _postRepository.Update(post);

            await _unitOfWork.SaveChangesAsync();

            return comment.Id;
        }

        public async Task UpdateCommentAsync(Guid id, Guid commentId, UpdateCommentDto dto)
        {
            if (_tokenService.UserId is null)
                throw new Exception("User is not authorized!");

            var user = await _userRepository.FindAsync(_tokenService.UserId.Value);

            if (user is null)
                throw new UserException("Failed to find User!");

            var post = await _postRepository.FindAsync(id);

            if (post is null)
                throw new UserException("Post not found.");

            await _postRepository.LoadComment(post, commentId);

            var comment = post.UpdateComment(user.Id, commentId, dto);

            _postRepository.Update(post);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveCommentAsync(Guid id, Guid commentId)
        {
            if (_tokenService.UserId is null)
                throw new Exception("User is not authorized!");

            var user = await _userRepository.FindAsync(_tokenService.UserId.Value);

            if (user is null)
                throw new UserException("Failed to find User!");

            var post = await _postRepository.FindAsync(id);

            if (post is null)
                throw new UserException("Post not found.");

            await _postRepository.LoadComment(post, commentId);

            var comment = post.RemoveComment(user.Id, commentId);

            _postRepository.Update(post);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}