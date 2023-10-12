using AutoMapper;
using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.Exceptions;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Repositories;
using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.Services;

namespace NetCore7API.Services
{
    public class CommentService : Domain.Services.ICommentService
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(
            IRepository<Comment> commentRepository,
            IUserRepository userRepository,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<CommentDto, CommentFilter>> ListAsync(CommentFilter filter)
        {
            var response = await _commentRepository.ListAsync(filter);

            var totalCount = await _commentRepository.TotalCountAsync(filter);

            var mappedResponse = _mapper.Map<IEnumerable<CommentDto>>(response);

            return new PagedResponse<CommentDto, CommentFilter>(mappedResponse, filter, totalCount);
        }

        public async Task<CommentDto> GetAsync(Guid id)
        {
            var comment = await _commentRepository.FindAsync(id);

            if (comment is null)
                throw new Exception("Comment not found.");

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<CommentDto> CreateAsync(CreateCommentDto dto)
        {
            var user = await _userRepository.FindAsync(_tokenService.UserId);

            if (user is null)
                throw new UserException("Cannot find User");

            var comment = new Comment(dto.PostId, user.Id, dto.Text);

            _commentRepository.Add(comment);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<CommentDto> UpdateAsync(Guid id, UpdateCommentDto dto)
        {
            var comment = await _commentRepository.FindAsync(id);

            if (comment is null)
                throw new Exception("Comment not found.");

            if (comment.UserId != _tokenService.UserId)
                throw new UserException("You are not authorized to modify this comment.");

            comment.Update(dto);

            _commentRepository.Update(comment);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<CommentDto> HideAsync(Guid id, HideCommentDto dto)
        {
            var comment = await _commentRepository.FindAsync(id);

            if (comment is null)
                throw new Exception("Comment not found.");

            if (comment.UserId != _tokenService.UserId)
                throw new UserException("You are not authorized to modify this comment.");

            comment.Hide(dto);

            _commentRepository.Update(comment);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<CommentDto> DeleteAsync(Guid id)
        {
            var comment = await _commentRepository.FindAsync(id);

            if (comment is null)
                throw new Exception("Comment not found.");

            if (comment.UserId != _tokenService.UserId)
                throw new UserException("You are not authorized to modify this comment.");

            _commentRepository.SoftDelete(comment);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CommentDto>(comment);
        }
    }
}